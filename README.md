# Flotix Platform (flotix-backend-preview)

## Overview

Flotix Platform is the ASP.NET Core REST API of Flotix, a fleet management solution for owners, drivers and mechanics.
It implements the services of the project report (chapter 4, Solution Software Design) and keeps the json-server fake
API in [`jsonserver/`](jsonserver/) as functional reference. It is organized in bounded contexts following a layered
Domain-Driven Design structure with Entity Framework Core persistence on PostgreSQL.

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
    - Aggregates, value objects (e.g. `LicensePlate`, `SerialNumber`, `EmailAddress`), commands, queries and repository contracts.
    - `User` is an abstract aggregate specialized by `Owner`, `Driver` and `Mechanic` and created with the `User.Create` factory method.
    - Business rules live in the aggregates (e.g. the mileage of a vehicle cannot decrease, a delivered order is final, a driver must have a license, the maintenance lifecycle only allows valid transitions).
- **Application layer**
    - Command and query services per aggregate (CQS) that return explicit outcomes with `Result<TValue, TError>` and error enums.
    - Outbound services (`IHashingService`, `ITokenService`, `ITelemetryDataQueue`, `IVehicleLocationNotifier`).
    - Anti-corruption layer facades (`IFleetContextFacade`, `IFuelContextFacade`, `IMaintenanceContextFacade`) to read other contexts.
- **Interfaces layer**
    - REST controllers with `[controller]` routes converted to kebab-case, resources and static assemblers.
    - SignalR hubs for real-time monitoring (`/hubs/monitoring`) and alerts & notifications (`/hubs/notifications`).
- **Infrastructure layer**
    - EF Core repositories, one PostgreSQL schema per bounded context, foreign keys with restricted deletes, `snake_case` plural naming and audit timestamps through an interceptor.
    - IAM request authorization middleware, password hashing and JWT tokens.
    - IoT Telemetry Processor: a bounded in-memory queue and a background service that processes telemetry packets.

## Features

- API base route: `/api/v1/<resource>` with kebab-case resource names (e.g. `/api/v1/fuel-records`, `/api/v1/maintenance-requests`).
- Resources limited to the tables of the report database design: users, vehicles, fuel records, maintenance requests,
  incidents, IoT devices, telemetry data, alerts, notifications, IoT orders and reports.
- Sign-up and sign-in (`/api/v1/authentication/sign-up`, `/api/v1/authentication/sign-in`) with hashed passwords and role-based JWT.
- IAM request authorization middleware with `[AllowAnonymous]` and `[Authorize(Role...)]`, **disconnected by default** (see below).
- Sprint 3 technical stories:
    - **TS01** vehicle registration and lookup: `POST /api/v1/vehicles`, `GET /api/v1/vehicles`, `GET /api/v1/vehicles/{id}`.
    - **TS02** IoT telemetry ingestion: `POST /api/v1/telemetry-data` answers `202 Accepted`; the IoT Telemetry Processor stores
      the packet, marks the device as connected and publishes `VehicleLocationUpdated`. Corrupt packets return `400` and
      packets of unlinked devices are discarded; both are written to the error log.
    - **TS03** maintenance flow between owner and mechanic: `POST /api/v1/maintenance-requests`, then
      `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote`, `/budget-decision`, `/repair-start` and `/completion`.
      Invalid transitions return `409`.
    - **TS04** report generation: `POST /api/v1/reports` with `ownerId`, `reportType` (`Reporte de consumo de combustible`
      or `Reporte de costos de mantenimiento`), `from` and `to`; returns the lines per vehicle and totals, or an informative
      message when there are no records in the period.
- Real-time vehicle locations, alerts and per-user notifications with SignalR.
- Localized validation, error messages and OpenAPI documentation (`en`, `en-US`, `es`, `es-PE`) through `Accept-Language`.
- Standardized errors: `400`, `401`, `404`, `409` with localized messages and `500` as RFC 7807 `ProblemDetails`.

## Technologies

- C# 14 and .NET 10 (`net10.0`, LTS)
- ASP.NET Core Web API (REST controllers + built-in OpenAPI generation) and SignalR
- Scalar.AspNetCore for interactive API documentation
- Entity Framework Core 10 + PostgreSQL provider (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- Humanizer for table name pluralization
- ASP.NET Core Identity password hasher and `Microsoft.AspNetCore.Authentication.JwtBearer` (JWT HS256)
- Localization with `.resx` resources
- PostgreSQL in Docker (`docker-compose.yml`)
- PlantUML (architecture diagrams in `docs/`)

## Technical stories

The technical stories of the report (TS01 to TS31), with their implementation status and endpoints, are in
[`docs/user-stories.md`](docs/user-stories.md).

## Class diagram

A PlantUML class diagram that mirrors the report class diagram is available at [`docs/class-diagram.puml`](docs/class-diagram.puml).

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

The application automatically applies pending EF Core migrations on startup via `Database.Migrate()`. Each bounded
context owns a schema (`iam`, `fleet`, `fuel`, `maintenance`, `incidents`, `monitoring`, `notifications`, `commerce`,
`analytics`) and the relations of the report are foreign keys with `ON DELETE RESTRICT`: deleting a row that is still
referenced (e.g. a vehicle with fuel records) returns `409`.

To add a migration after changing the model:

```bash
dotnet tool restore
dotnet ef migrations add <MigrationName> --project FlotixPlatform.API
```

### 5) Explore the API with Scalar

- `http://localhost:5080/scalar`
- `https://localhost:7080/scalar`

The generated OpenAPI document is available at `/openapi/v1.json`. Swagger is not used.

### Production-style configuration

`FlotixPlatform.API/appsettings.Production.json` supports environment variable expansion:

- `%DATABASE_HOST%`, `%DATABASE_PORT%`, `%DATABASE_NAME%`
- `%DATABASE_USER%`, `%DATABASE_PASSWORD%`

The JWT signing secret is not stored in production settings: provide it with the `TokenSettings__Secret` environment
variable (at least 32 bytes). The API does not start without a valid secret.

Allowed front-end origins are configured in `Cors:Origins`. Set `ASPNETCORE_ENVIRONMENT=Production` before running
with production settings. A `Dockerfile` is provided to build and run the API.

## IAM (Identity, Profiles & Security)

Sign-up and sign-in are always available; sign-in returns a JWT (HS256) with the user id and role, configured in
`TokenSettings` (`Secret`, `Issuer`, `Audience`, `ExpirationHours`). Endpoint protection is done by `RequestAuthorizationMiddleware`, which is
**commented out** in `Program.cs` (`// app.UseRequestAuthorization();`). Once enabled:

- every endpoint not marked `[AllowAnonymous]` requires `Authorization: Bearer <token>`;
- `[Authorize(Role.Owner)]` restricts IoT orders and reports to fleet owners;
- SignalR hubs read the token from the `access_token` query parameter, and notifications are delivered only to their recipient.

## Real-time (SignalR)

| Hub | Event | Sent when |
| --- | --- | --- |
| `/hubs/monitoring` | `VehicleLocationUpdated` | the IoT Telemetry Processor stores a telemetry packet |
| `/hubs/notifications` | `AlertActivated` | an alert is created |
| `/hubs/notifications` | `NotificationReceived` | a notification is created (only to its user) |

## Error responses

- **202 Accepted**: a telemetry packet was queued for processing.
- **400 Bad Request**: request validation failed or a business rule rejected the data; returns a localized message.
- **401 Unauthorized**: invalid credentials, or a missing/invalid token when the IAM middleware is enabled.
- **404 Not Found**: resource not found, or a referenced resource (vehicle, user, alert...) does not exist.
- **409 Conflict**: duplicates (plate, serial number, email, a second device for a vehicle), invalid state transitions
  (maintenance lifecycle, final order) or deleting a resource that is still referenced.
- **500 Internal Server Error**: unexpected server error (RFC 7807 `ProblemDetails` format).

## Build and verify

```bash
dotnet build flotix-platform.sln
```
