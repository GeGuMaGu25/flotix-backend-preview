# Flotix Platform (flotix-backend-preview)

## Overview

Flotix Platform is the ASP.NET Core REST API of Flotix, a fleet management solution for owners, drivers and mechanics.
It replaces the json-server fake API kept in [`jsonserver/`](jsonserver/) as functional reference, and is organized in
bounded contexts following a layered Domain-Driven Design structure with Entity Framework Core persistence on PostgreSQL.

## Domain-Driven Design approach

- **Bounded contexts**
    - `IAM`: Identity, Profiles & Security.
    - `Fleet`: Vehicle & Fleet Management.
    - `Fuel`: Fuel Control.
    - `Maintenance`: Maintenance Management.
    - `Incidents`: Incident Management.
    - `Monitoring`: Real-Time Monitoring / Fleet Tracking.
    - `Notifications`: Alerts & Notifications.
    - `Commerce`: Digital Experience / IoT Commerce.
    - `Analytics`: Reporting & Analytics.
    - `Shared`: cross-cutting abstractions used by every context, such as repository contracts, unit of work, auditing, naming conventions and the `Result` pattern.
- **Domain layer**
    - Aggregates, value objects (e.g. `LicensePlate`, `Sku`, `EmailAddress`), commands, queries and repository contracts.
    - Business rules live in the aggregates (e.g. the mileage of a vehicle cannot decrease, a delivered order is final, a driver must have a license).
- **Application layer**
    - Command and query services per aggregate (CQS) that return explicit outcomes with `Result<TValue, TError>` and error enums.
    - Outbound services (`IHashingService`, `ITokenService`) for the IAM context.
- **Interfaces layer**
    - REST controllers with `[controller]` routes converted to kebab-case, resources and static assemblers.
    - SignalR hubs for real-time monitoring (`/hubs/monitoring`) and alerts & notifications (`/hubs/notifications`).
- **Infrastructure layer**
    - EF Core repositories, per-context model configuration, `snake_case` plural naming, audit timestamps through an interceptor.
    - IAM request authorization middleware, password hashing and Data Protection bearer tokens.

## Features

- API base route: `/api/v1/<resource>` with kebab-case resource names (e.g. `/api/v1/fuel-records`, `/api/v1/iot-catalog`).
- Create, list (with optional filters such as `?vehicleId=`), retrieve, update and delete for every resource of the fake API.
- Sign-up and sign-in (`/api/v1/authentication/sign-up`, `/api/v1/authentication/sign-in`) with hashed passwords and bearer tokens.
- IAM request authorization middleware with `[AllowAnonymous]` and `[Authorize(Role...)]`, **disconnected by default** (see below).
- Real-time telemetry, alerts and per-user notifications with SignalR.
- IoT orders priced on the server from the IoT catalog (the client never sends prices).
- Localized validation, error messages and OpenAPI documentation (`en`, `en-US`, `es`, `es-PE`) through `Accept-Language`.
- Standardized errors: `400`, `401`, `404`, `409` with localized messages and `500` as RFC 7807 `ProblemDetails`.

## Technologies

- C# 14 and .NET 10 (`net10.0`, LTS)
- ASP.NET Core Web API (REST controllers + built-in OpenAPI generation) and SignalR
- Scalar.AspNetCore for interactive API documentation
- Entity Framework Core 10 + PostgreSQL provider (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- Humanizer for table name pluralization
- ASP.NET Core Identity password hasher and Data Protection (IAM, no third-party security dependencies)
- Localization with `.resx` resources
- PostgreSQL in Docker (`docker-compose.yml`)
- PlantUML (architecture diagrams in `docs/`)

## Technical stories

The API-focused technical stories for frontend integration are in [`docs/user-stories.md`](docs/user-stories.md).

## Class diagram

A PlantUML class diagram of the bounded contexts is available at [`docs/class-diagram.puml`](docs/class-diagram.puml).

## Architecture decisions

The architecture decision records are in [`docs/adrs.md`](docs/adrs.md).

## Getting started (quick)

### Prerequisites

- .NET SDK 10
- Docker (or any reachable PostgreSQL instance)

### 1) Start PostgreSQL

```bash
docker compose up -d
```

### 2) Restore dependencies

```bash
dotnet restore flotix-platform.sln
```

### 3) Configure connection string

By default, development uses `FlotixPlatform.API/appsettings.Development.json`, matching `docker-compose.yml`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=flotix;Username=flotix;Password=flotix"
}
```

### 4) Run the API

```bash
dotnet run --project FlotixPlatform.API
```

### Database initialization

The application automatically applies pending EF Core migrations on startup via `Database.Migrate()`; the IoT catalog
and the fleet preferences are seeded by the initial migration.

To add a migration after changing the model:

```bash
dotnet tool restore
dotnet ef migrations add <MigrationName> --project FlotixPlatform.API
```

### 5) Explore the API with Scalar

- `http://localhost:5080/scalar`
- `https://localhost:7080/scalar`

The generated OpenAPI document is available at `/openapi/v1.json`.

### Production-style configuration

`FlotixPlatform.API/appsettings.Production.json` supports environment variable expansion:

- `%DATABASE_HOST%`, `%DATABASE_PORT%`, `%DATABASE_NAME%`
- `%DATABASE_USER%`, `%DATABASE_PASSWORD%`

Allowed front-end origins are configured in `Cors:Origins`. Set `ASPNETCORE_ENVIRONMENT=Production` before running
with production settings. A `Dockerfile` is provided to build and run the API.

## IAM (Identity, Profiles & Security)

Sign-up and sign-in are always available. Endpoint protection is done by `RequestAuthorizationMiddleware`, which is
**commented out** in `Program.cs` (`// app.UseRequestAuthorization();`). Once enabled:

- every endpoint not marked `[AllowAnonymous]` requires `Authorization: Bearer <token>`;
- `[Authorize(Role.Owner)]` restricts IoT orders and reports to fleet owners;
- SignalR hubs read the token from the `access_token` query parameter, and notifications are delivered only to their recipient.

## Real-time (SignalR)

| Hub | Event | Sent when |
| --- | --- | --- |
| `/hubs/monitoring` | `TelemetryRecordReceived` | a telemetry record is posted |
| `/hubs/notifications` | `AlertActivated` | an alert is created |
| `/hubs/notifications` | `NotificationReceived` | a notification is created (only to its user) |

## Error responses

- **400 Bad Request**: request validation failed or a business rule rejected the data; returns a localized message.
- **401 Unauthorized**: invalid credentials, or a missing/invalid token when the IAM middleware is enabled.
- **404 Not Found**: resource not found.
- **409 Conflict**: duplicates (plate, serial number, SKU, email, notification preference) or state conflicts (closed maintenance request, final order).
- **500 Internal Server Error**: unexpected server error (RFC 7807 `ProblemDetails` format).

## Build and verify

```bash
dotnet build flotix-platform.sln
```
