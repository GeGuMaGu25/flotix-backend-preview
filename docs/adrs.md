# Architecture Decision Records (ADRs)

## About this document

This document records the significant architectural decisions of the Flotix Platform REST API. Each record follows the
lightweight format proposed by Michael Nygard: **Context**, **Decision**, **Consequences**, plus a **Status** and **Date**.

- One decision per record; records are short and focused on the *why*.
- Records are immutable once **Accepted**. To change a decision, add a new ADR that **Supersedes** the old one.
- Statuses: `Proposed`, `Accepted`, `Deprecated`, `Superseded by ADR-XXX`.

## Index

| ADR | Title | Status |
|-----|-------|--------|
| [ADR-001](#adr-001) | Organize the code by bounded context with a layered DDD structure | Accepted |
| [ADR-002](#adr-002) | Reference other bounded contexts by identifier only | Accepted |
| [ADR-003](#adr-003) | Separate command and query services (CQS) | Accepted |
| [ADR-004](#adr-004) | Model expected failures with a `Result` type | Accepted |
| [ADR-005](#adr-005) | Use value objects for validated identifiers | Accepted |
| [ADR-006](#adr-006) | Map REST resources with static assemblers | Accepted |
| [ADR-007](#adr-007) | Persist with EF Core on PostgreSQL using Repository and Unit of Work | Accepted |
| [ADR-008](#adr-008) | Use `snake_case` database naming and automatic audit timestamps | Accepted |
| [ADR-009](#adr-009) | Apply database migrations on startup | Accepted |
| [ADR-010](#adr-010) | Use lowercase kebab-case routes | Accepted |
| [ADR-011](#adr-011) | Report errors as RFC 7807 `ProblemDetails` | Accepted |
| [ADR-012](#adr-012) | Localize user-facing text with `.resx` resources | Accepted |
| [ADR-013](#adr-013) | Keep the Spanish enum values of the fake API on the wire | Accepted |
| [ADR-014](#adr-014) | Use built-in OpenAPI and Scalar instead of Swashbuckle | Accepted |
| [ADR-015](#adr-015) | Push real-time events with SignalR | Accepted |
| [ADR-016](#adr-016) | Authorize requests with an IAM middleware, disconnected by default | Accepted |
| [ADR-017](#adr-017) | Price IoT orders on the server | Accepted |

---

## ADR-001

**Organize the code by bounded context with a layered DDD structure**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The API covers nine business capabilities taken from the fake API (identity, fleet, fuel, maintenance, incidents, monitoring, alerts and notifications, IoT commerce and reporting). We need a structure that keeps domain rules independent from frameworks and prevents unrelated features from becoming entangled.

### Decision
Use a single deployable project organized first by bounded context (`IAM`, `Fleet`, `Fuel`, `Maintenance`, `Incidents`, `Monitoring`, `Notifications`, `Commerce`, `Analytics`, `Shared`) and then by layer:

- `Domain` — aggregates, value objects, commands, queries, repository contracts.
- `Application` — service contracts (`Services`), implementations (`Internal`), outbound services and application errors.
- `Infrastructure` — persistence, model configuration and other technical adapters.
- `Interfaces` — REST controllers, resources, assemblers and SignalR hubs.

Dependencies point inward. Cross-context technical concerns live in `Shared`.

### Consequences
- (+) Domain logic is testable without ASP.NET Core or EF Core.
- (+) Each capability evolves in its own folder.
- (−) More folders and types than a simple CRUD layout.
- (−) The dependency rule is enforced by convention only.

---

## ADR-002

**Reference other bounded contexts by identifier only**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Aggregates relate across contexts (a fuel record belongs to a vehicle and a driver, an order to an owner).

### Decision
Aggregates keep plain integer identifiers of aggregates from other contexts (`VehicleId`, `OwnerId`, `UserId`...) without navigation properties or cross-context foreign keys. Each context only indexes the identifiers it filters by.

### Consequences
- (+) Contexts stay decoupled and could be split later.
- (−) Referential integrity across contexts is not enforced by the database; an anti-corruption layer (ACL) can validate references when required.

---

## ADR-003

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

## ADR-004

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

## ADR-005

**Use value objects for validated identifiers**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Plates, serial numbers, SKUs, phone numbers and email addresses are strings with format rules.

### Decision
Model them as immutable value objects (`LicensePlate`, `SerialNumber`, `Sku`, `PhoneNumber`, `EmailAddress`) that validate and normalize on construction. EF Core maps them with value converters; raw strings are only accepted at the API boundary.

### Consequences
- (+) Invalid values cannot enter the domain and duplicates are detected on normalized values.
- (−) Requires value converters and mapping code.

---

## ADR-006

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

## ADR-007

**Persist with EF Core on PostgreSQL using Repository and Unit of Work**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The service needs relational persistence with unique constraints, arrays (workshop tags) and JSON documents (report filters).

### Decision
Use Entity Framework Core with the Npgsql provider and a PostgreSQL instance in Docker. Repository interfaces live in the domain, implementations in infrastructure, and changes are committed through `IUnitOfWork.CompleteAsync`. Each context contributes its model through an `Apply<Context>Configuration` extension.

### Consequences
- (+) Domain and application code depend on abstractions, not EF Core.
- (+) Unique indexes back the duplicate rules at database level.
- (−) Switching databases requires changing the provider and re-validating migrations.

---

## ADR-008

**Use `snake_case` database naming and automatic audit timestamps**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Database names should follow PostgreSQL conventions and every aggregate should record when it was created and updated.

### Decision
Convert table, column, key and index names to `snake_case` (tables pluralized with Humanizer) and set `created_at`/`updated_at` through `AuditableEntityInterceptor` for every `IAuditableEntity`.

### Consequences
- (+) Consistent schema without per-entity configuration.
- (−) Naming is applied by convention at model creation.

---

## ADR-009

**Apply database migrations on startup**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The API should be runnable with a single command in development and in containers.

### Decision
Call `Database.Migrate()` on startup. Reference data (IoT catalog, fleet preferences) is seeded through migrations.

### Consequences
- (+) Simple deployments.
- (−) Concurrent replicas may race on the first start; run migrations as a separate step if that becomes a problem.

---

## ADR-010

**Use lowercase kebab-case routes**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
REST naming favors lowercase, hyphen-separated resource names, matching the fake API routes.

### Decision
Controllers use `[Route("api/v1/[controller]")]` and `KebabCaseRouteNamingConvention` converts the controller token to kebab-case (`FuelRecords` → `fuel-records`).

### Consequences
- (+) The front end keeps the same resource names as the fake API.
- (−) Routes depend on controller names.

---

## ADR-011

**Report errors as RFC 7807 `ProblemDetails`**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Clients need a consistent error format.

### Decision
Validation errors return localized `ValidationProblemDetails`, expected failures return localized messages with the matching status code and unexpected failures return `ProblemDetails` with status 500.

### Consequences
- (+) Predictable error handling for clients.
- (−) Expected failures return plain localized strings, as in the reference project.

---

## ADR-012

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

## ADR-013

**Keep the Spanish enum values of the fake API on the wire**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The front end already uses the Spanish values of the fake API (e.g. `En mantenimiento`, `Pendiente de pago`).

### Decision
Enums are English in code, persisted by member name and serialized with `JsonStringEnumMemberName` using the Spanish values.

### Consequences
- (+) No front-end changes for status values.
- (−) Renaming a wire value is a breaking API change.

---

## ADR-014

**Use built-in OpenAPI and Scalar instead of Swashbuckle**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
.NET 10 generates OpenAPI documents natively.

### Decision
Use `Microsoft.AspNetCore.OpenApi` with localized operation and schema transformers per context, and Scalar as API explorer in every environment.

### Consequences
- (+) Fewer dependencies.
- (−) Transformers must be registered per bounded context.

---

## ADR-015

**Push real-time events with SignalR**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
Fleet tracking, alerts and notifications must reach dashboards without polling.

### Decision
Use ASP.NET Core SignalR with strongly typed hubs per context (`MonitoringHub`, `NotificationsHub`). Controllers push the created resource after a successful command; notifications are sent only to their user.

### Consequences
- (+) Built into ASP.NET Core, no extra dependency.
- (−) Per-user delivery requires the IAM middleware to identify connections.

---

## ADR-016

**Authorize requests with an IAM middleware, disconnected by default**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The platform needs user management and endpoint protection, but the front end still runs against open endpoints.

### Decision
Implement `RequestAuthorizationMiddleware` with `[AllowAnonymous]` and `[Authorize(Role...)]` metadata, PBKDF2 password hashing (ASP.NET Core Identity hasher) and time-limited Data Protection bearer tokens. The middleware is registered but commented out in `Program.cs` until the front end sends tokens.

### Consequences
- (+) No third-party security dependencies; enabling it is a one-line change.
- (−) While disconnected, every endpoint is public.
- (−) The Data Protection key ring must be persisted in production.

---

## ADR-017

**Price IoT orders on the server**

- **Status:** Accepted
- **Date:** 2026-10-28

### Context
The fake API stored the total amount sent by the client, which allows price tampering.

### Decision
`IotOrder` computes its total from the catalog unit price and starts as `PendingPayment`; delivered or cancelled orders are final.

### Consequences
- (+) Prices cannot be tampered with by clients.
- (−) The client cannot set a custom price.
