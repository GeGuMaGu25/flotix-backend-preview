# Architecture Decision Records (ADRs)

## About this document

This document records the significant architectural decisions of the Flotix Platform REST API, aligned with chapter 4 (Solution Software Design) of the project report. Each record follows the
lightweight format proposed by Michael Nygard: **Context**, **Decision**, **Consequences**, plus a **Status** and **Date**.

- One decision per record; records are short and focused on the *why*.
- Records are immutable once **Accepted**. To change a decision, add a new ADR that **Supersedes** the old one.
- Statuses: `Proposed`, `Accepted`, `Deprecated`, `Superseded by ADR-XXX`.

## Index

| ADR | Title | Status |
|-----|-------|--------|
| [ADR-001](#adr-001) | Organize the code by bounded context with a layered DDD structure | Accepted |
| [ADR-002](#adr-002) | Separate command and query services (CQS) | Accepted |
| [ADR-003](#adr-003) | Model expected failures with a `Result` type | Accepted |
| [ADR-004](#adr-004) | Use value objects for validated identifiers | Accepted |
| [ADR-005](#adr-005) | Map REST resources with static assemblers | Accepted |
| [ADR-006](#adr-006) | Persist with EF Core on PostgreSQL using Repository and Unit of Work | Accepted |
| [ADR-007](#adr-007) | Use one database schema per bounded context and foreign keys with restricted deletes | Accepted |
| [ADR-008](#adr-008) | Use UUID identifiers and the column names of the report | Accepted |
| [ADR-009](#adr-009) | Store users with table-per-hierarchy inheritance | Accepted |
| [ADR-010](#adr-010) | Use `snake_case` database naming and automatic audit timestamps | Accepted |
| [ADR-011](#adr-011) | Apply database migrations on startup | Accepted |
| [ADR-012](#adr-012) | Use REST controllers with lowercase kebab-case routes | Accepted |
| [ADR-013](#adr-013) | Report errors as RFC 7807 `ProblemDetails` | Accepted |
| [ADR-014](#adr-014) | Localize user-facing text with `.resx` resources | Accepted |
| [ADR-015](#adr-015) | Use the Spanish status values of the report on the wire | Accepted |
| [ADR-016](#adr-016) | Document the API with OpenAPI and Scalar, without Swagger | Accepted |
| [ADR-017](#adr-017) | Authenticate with role-based JWT and an IAM middleware, disconnected by default | Accepted |
| [ADR-018](#adr-018) | Process IoT telemetry in the background | Accepted |
| [ADR-019](#adr-019) | Push real-time events with SignalR | Accepted |
| [ADR-020](#adr-020) | Model the maintenance lifecycle with explicit commands | Accepted |
| [ADR-021](#adr-021) | Read data of other bounded contexts through ACL facades | Accepted |
| [ADR-022](#adr-022) | Limit the persistence model to the database design of the report | Accepted |

---

## ADR-001

**Organize the code by bounded context with a layered DDD structure**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The report (chapter 4) defines nine bounded contexts: Identity, Profiles & Security; Vehicle & Fleet Management; Fuel Control; Maintenance Management; Incident Management; Real-Time Monitoring / Fleet Tracking; Alerts & Notifications; Digital Experience / IoT Commerce; Reporting & Analytics. We need a structure that keeps domain rules independent from frameworks without the ceremony of hexagonal or clean architecture.

### Decision
Use a single deployable monolith organized first by bounded context (`IAM`, `Fleet`, `Fuel`, `Maintenance`, `Incidents`, `Monitoring`, `Notifications`, `Commerce`, `Analytics`, `Shared`) and then by layer, following the structure of the reference course project:

- `Domain` — aggregates, value objects, commands, queries, repository contracts.
- `Application` — service contracts, implementations (`Internal`), outbound services, ACL facades and application errors.
- `Infrastructure` — persistence, model configuration and technical adapters (JWT, hashing, queues, SignalR notifiers).
- `Interfaces` — REST controllers, resources, assemblers, SignalR hubs and ACL facade contracts.

Dependencies point to abstractions; cross-context technical concerns live in `Shared`.

### Consequences
- (+) Domain logic is testable without ASP.NET Core or EF Core.
- (+) Each bounded context evolves in its own folder.
- (−) More folders and types than a simple CRUD layout.
- (−) The dependency rule is enforced by convention only.

---

## ADR-002

**Separate command and query services (CQS)**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Writes have validation, uniqueness rules and persistence side effects, while reads are simple lookups.

### Decision
Expose two application services per aggregate (e.g. `IVehicleCommandService`, `IVehicleQueryService`). Commands and queries are immutable records and every handler accepts a `CancellationToken`.

### Consequences
- (+) Clear intent and smaller, focused classes.
- (−) Slightly more boilerplate per use case.
- This is CQS at the service level, not full CQRS.

---

## ADR-003

**Model expected failures with a `Result` type**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Duplicates, unknown references and broken business rules are expected outcomes, not exceptional conditions.

### Decision
Command handlers return `Result<TValue, TError>` with an error enum per command (e.g. `UpdateVehicleError`). Aggregates throw `ArgumentException` for invalid data and `InvalidOperationException` for broken rules; the command service translates them into errors and the REST layer maps them to HTTP status codes through `ActionResultFrom...ResultAssembler` classes.

### Consequences
- (+) Failure modes are explicit and exhaustively mappable to HTTP status codes.
- (−) Callers must handle both branches.

---

## ADR-004

**Use value objects for validated identifiers**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
License plates, serial numbers and email addresses are strings with format rules and must be unique.

### Decision
Model them as immutable value objects (`LicensePlate`, `SerialNumber`, `EmailAddress`) that validate and normalize on construction. EF Core maps them with value converters to the report columns (`license_plate VARCHAR(15)`, `serial_number`, `email`); raw strings are only accepted at the API boundary.

### Consequences
- (+) Invalid values cannot enter the domain and duplicates are detected on normalized values.
- (−) Requires value converters and mapping code.

---

## ADR-005

**Map REST resources with static assemblers**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Domain objects must not be exposed directly through the API (e.g. the password hash) and controllers should stay thin.

### Decision
Use dedicated resources and static assemblers in `Interfaces/Rest/Transform`, named `<Target>From<Source>Assembler`.

### Consequences
- (+) The public contract is decoupled from the domain model.
- (−) No automatic mapping; each resource needs an assembler.

---

## ADR-006

**Persist with EF Core on PostgreSQL using Repository and Unit of Work**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Section 4.8 of the report defines a relational PostgreSQL database.

### Decision
Use Entity Framework Core with the Npgsql provider and a PostgreSQL instance in Docker. Repository interfaces live in the domain, implementations in infrastructure, and changes are committed through `IUnitOfWork.CompleteAsync`. Each context contributes its model through an `Apply<Context>Configuration` extension.

### Consequences
- (+) Domain and application code depend on abstractions, not EF Core.
- (+) Unique indexes back the duplicate rules at database level.
- (−) Switching databases requires changing the provider and re-validating migrations.

---

## ADR-007

**Use one database schema per bounded context and foreign keys with restricted deletes**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The database design of the report (4.8) groups the tables by bounded context and relates them with foreign keys (`vehicles.owner_id → users.id`, `fuel_records.vehicle_id → vehicles.id`, `notifications.alert_id → alerts.id`, ...). The report also justifies the history tables by traceability.

### Decision
Each bounded context owns a PostgreSQL schema (`iam`, `fleet`, `fuel`, `maintenance`, `incidents`, `monitoring`, `notifications`, `commerce`, `analytics`) and the relations of the report are real foreign keys with `ON DELETE RESTRICT`. Aggregates still reference other contexts by identifier only (no navigation properties); the foreign keys are configured without navigations (`HasOne<Vehicle>().WithMany().HasForeignKey(...)`). A foreign key violation (SQLSTATE `23503`) is mapped to `404` when creating or updating and to `409` when deleting a referenced row.

### Consequences
- (+) The schema matches the report and the database guarantees referential integrity.
- (+) Deleting a vehicle or user with history is rejected instead of losing records (traceability).
- (−) Contexts are coupled at the database level; splitting one out requires removing its foreign keys.
- (−) Rows with dependents must be cleaned up explicitly before deletion.

---

## ADR-008

**Use UUID identifiers and the column names of the report**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Every table of the database design (4.8) uses a `UUID` primary key and English column names (`license_plate`, `current_mileage`, `mileage_at_refill`, `recorded_date`, ...).

### Decision
Aggregates use `Guid` identifiers generated by the application and properties named after the report columns. `created_at` and `updated_at` are the only technical columns added to the report design (see ADR-010).

### Consequences
- (+) Identifiers are not guessable and can be generated before persisting.
- (−) Larger keys than integers.

---

## ADR-009

**Store users with table-per-hierarchy inheritance**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The report models `User` as a base class specialized by `Owner`, `Driver` and `Mechanic` (Factory Method) and stores them in a single `users` table with a `role_type` column.

### Decision
`User` is an abstract aggregate with `Owner` (`company_name`), `Driver` (`license_number`, `license_expiration`) and `Mechanic` (`workshop_name`) subclasses mapped with TPH; `role_type` is the discriminator. `User.Create` is the factory method that builds the subclass for a role, and a driver requires a license number and expiration.

### Consequences
- (+) One table as in the report; role-specific rules live in each subclass.
- (−) Role-specific columns are nullable at database level.

---

## ADR-010

**Use `snake_case` database naming and automatic audit timestamps**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Database names should follow PostgreSQL conventions and every aggregate should record when it was created and updated.

### Decision
Convert table, column, key and index names to `snake_case` (tables pluralized with Humanizer) and set `created_at`/`updated_at` through `AuditableEntityInterceptor` for every `IAuditableEntity`. The audit columns are not in the report diagram and are kept as technical columns.

### Consequences
- (+) Consistent schema without per-entity configuration.
- (−) Naming is applied by convention at model creation.

---

## ADR-011

**Apply database migrations on startup**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The API should be runnable with a single command in development and in containers.

### Decision
Call `Database.Migrate()` on startup.

### Consequences
- (+) Simple deployments.
- (−) Concurrent replicas may race on the first start; run migrations as a separate step if that becomes a problem.

---

## ADR-012

**Use REST controllers with lowercase kebab-case routes**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
REST naming favors lowercase, hyphen-separated resource names. Section 4.6.3 of the report mentions Minimal APIs, while its C4 component diagrams name the components as controllers (e.g. `Vehicle Controller`), and the project requirement asks for controllers with `[controller]` routes.

### Decision
Controllers use `[Route("api/v1/[controller]")]` and `KebabCaseRouteNamingConvention` converts the controller token to kebab-case (`FuelRecords` → `fuel-records`). Endpoints are documented with OpenAPI regardless of the hosting model.

### Consequences
- (+) Matches the C4 components and the reference course project.
- (−) Deviates from the Minimal APIs wording of 4.6.3; the HTTP contract is the same.

---

## ADR-013

**Report errors as RFC 7807 `ProblemDetails`**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Clients need a consistent error format; the acceptance criteria of the technical stories expect `400`, `404` and `409` with an error detail.

### Decision
Validation errors return localized `ValidationProblemDetails`, expected failures return localized messages with the matching status code and unexpected failures return `ProblemDetails` with status 500.

### Consequences
- (+) Predictable error handling for clients.
- (−) Expected failures return plain localized strings, as in the reference project.

---

## ADR-014

**Localize user-facing text with `.resx` resources**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Users of the platform speak Spanish; developers read English documentation.

### Decision
Localize messages, validation errors and OpenAPI metadata with `SharedResource` files for `en`, `en-US`, `es` and `es-PE`, selected by `Accept-Language`.

### Consequences
- (+) The same API serves both languages.
- (−) Every new message needs entries in every resource file.

---

## ADR-015

**Use the Spanish status values of the report on the wire**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The technical stories use Spanish status values (e.g. `En mantenimiento`, `Pendiente de pago`, `Reporte de consumo de combustible`) that the front end displays.

### Decision
Enums are English in code, persisted by member name and serialized with `JsonStringEnumMemberName` using the Spanish values.

### Consequences
- (+) The API speaks the language of the stories and the front end.
- (−) Renaming a wire value is a breaking API change.

---

## ADR-016

**Document the API with OpenAPI and Scalar, without Swagger**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
TS31 requires documenting the RESTful API with OpenAPI. .NET 10 generates OpenAPI documents natively.

### Decision
Use `Microsoft.AspNetCore.OpenApi` with localized operation and schema transformers per context and Scalar (`/scalar`) as the API explorer in every environment. Swagger UI and Swashbuckle are not used.

### Consequences
- (+) Fewer dependencies; the specification is always in sync with the code.
- (−) Transformers must be registered per bounded context.

---

## ADR-017

**Authenticate with role-based JWT and an IAM middleware, disconnected by default**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The Auth Service of the report (4.6.3) issues JSON Web Tokens with role-based authorization for Owner, Driver and Mechanic. The front end still runs against open endpoints.

### Decision
Passwords are hashed with the ASP.NET Core Identity password hasher (PBKDF2) and `TokenService` issues HS256 JWTs with the user id and role, validated with `Microsoft.AspNetCore.Authentication.JwtBearer` (`TokenSettings`: secret of at least 32 bytes, issuer, audience, expiration). `RequestAuthorizationMiddleware` enforces `[AllowAnonymous]` and `[Authorize(Role...)]` and is registered but commented out in `Program.cs` until the front end sends tokens.

### Consequences
- (+) Stateless tokens as described in the report; enabling protection is a one-line change.
- (+) The application refuses to start without a valid secret.
- (−) While disconnected, every endpoint is public.
- (−) The production secret must be provided through configuration (e.g. the `TokenSettings__Secret` environment variable).

---

## ADR-018

**Process IoT telemetry in the background**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
TS02 requires receiving telemetry packets, storing them, updating the vehicle location in real time and discarding corrupt packets with an error log. The C4 component diagram of Real-Time Monitoring includes an `IoT Telemetry Processor`.

### Decision
`POST /api/v1/telemetry-data` validates the packet and enqueues it in a bounded `System.Threading.Channels` queue, returning `202 Accepted`. `TelemetryDataProcessor` (a `BackgroundService`) dequeues each packet, discards it with an error log when the device is not linked, otherwise stores it in `telemetry_data`, marks the device as connected and publishes `VehicleLocationUpdated` through SignalR.

### Consequences
- (+) Devices get a fast response and ingestion is decoupled from persistence.
- (−) Packets in the in-memory queue are lost if the process stops; a durable broker can replace the queue behind `ITelemetryDataQueue`.

---

## ADR-019

**Push real-time events with SignalR**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Fleet tracking, alerts and notifications must reach dashboards without polling.

### Decision
Use ASP.NET Core SignalR with strongly typed hubs per context (`MonitoringHub`, `NotificationsHub`). Monitoring publishes `VehicleLocationUpdated`; notifications publishes `AlertActivated` and `NotificationReceived`, the latter only to its user.

### Consequences
- (+) Built into ASP.NET Core, no extra dependency.
- (−) Per-user delivery requires the IAM middleware to identify connections.

---

## ADR-020

**Model the maintenance lifecycle with explicit commands**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
TS03 describes the flow between the owner and the mechanic: request, diagnostic and quote, budget approval or rejection, repair and completion, with state conflicts returning `409`.

### Decision
`MaintenanceRequest` exposes one method per transition (`SubmitQuote`, `Approve`, `Reject`, `StartRepair`, `Finish`) and the API one endpoint per command (`/diagnostic-and-quote`, `/budget-decision`, `/repair-start`, `/completion`) instead of a generic status update. Invalid transitions throw `InvalidOperationException` and are mapped to `409`.

### Consequences
- (+) The aggregate guards the lifecycle and the API expresses intent.
- (−) More endpoints than a generic `PUT`.

---

## ADR-021

**Read data of other bounded contexts through ACL facades**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
TS04 consolidates data of Fleet, Fuel and Maintenance into reports owned by Reporting & Analytics.

### Decision
Contexts that are read by others expose a facade contract in `Interfaces/ACL` (`IFleetContextFacade`, `IFuelContextFacade`, `IMaintenanceContextFacade`) implemented in `Application/ACL` with primitive types. Analytics depends only on these contracts.

### Consequences
- (+) Analytics does not depend on the domain model of other contexts.
- (−) Each new cross-context query requires a facade method.

---

## ADR-022

**Limit the persistence model to the database design of the report**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The fake API had resources (workshops, IoT catalog, notification and fleet preferences) that are not in the database design of the report (4.8).

### Decision
Only the tables of the report are implemented: `users`, `vehicles`, `fuel_records`, `maintenance_requests`, `incidents`, `iot_devices`, `telemetry_data`, `alerts`, `notifications`, `iot_orders` and `reports`. IoT orders store the `total_amount` sent by the client until a catalog exists.

### Consequences
- (+) The backend and the report stay consistent.
- (−) TS14 (workshops) and TS24 (IoT catalog) remain pending until the database design includes their tables.
