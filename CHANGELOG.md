# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- TS02: IoT telemetry ingestion with `202 Accepted`, a background IoT Telemetry Processor and the `VehicleLocationUpdated`
  SignalR event; corrupt packets and packets of unlinked devices are written to the error log.
- TS03: maintenance flow between owner and mechanic with diagnostic and quote, budget decision, repair start and
  completion commands; invalid transitions return `409`.
- TS04: generation of fuel consumption and maintenance cost reports by date range through anti-corruption layer facades
  of the Fleet, Fuel and Maintenance contexts.
- Role-based JWT (HS256) authentication with configurable `TokenSettings`.

### Changed

- **BREAKING:** the persistence model follows the database design of the report: UUID identifiers, the report table and
  column names, one PostgreSQL schema per bounded context and foreign keys with restricted deletes (referenced rows
  return `409` on delete, unknown references return `404`).
- **BREAKING:** users are stored with table-per-hierarchy inheritance (`Owner`, `Driver`, `Mechanic`) discriminated by
  `role_type`.
- **BREAKING:** telemetry, maintenance and report endpoints use the contracts of the technical stories.

### Removed

- **BREAKING:** Data Protection bearer tokens, replaced by JWT.
- **BREAKING:** workshops, IoT catalog, notification preferences and fleet preferences, which are not part of the
  database design of the report.

### Documentation

- Technical stories TS01 to TS31 of the report with their implementation status, ADRs, class diagram and README aligned
  with the project report.

## [1.0.0] - 2026-10-28

### Added

- Layered DDD structure with nine bounded contexts: Identity, Profiles & Security; Vehicle & Fleet Management; Fuel Control;
  Maintenance Management; Incident Management; Real-Time Monitoring / Fleet Tracking; Alerts & Notifications;
  Digital Experience / IoT Commerce; Reporting & Analytics.
- REST API covering every resource of the json-server fake API, with kebab-case routes under `/api/v1`.
- Sign-up and sign-in with hashed passwords and Data Protection bearer tokens, plus a request authorization middleware
  (disconnected by default) with anonymous and role-based endpoint metadata.
- SignalR hubs for real-time telemetry, alerts and per-user notifications.
- Entity Framework Core persistence with PostgreSQL, value objects, unique indexes, seeded IoT catalog and fleet preferences,
  and automatic creation/update audit timestamps.
- Localized validation responses, standardized API errors and localized OpenAPI documentation through Scalar.
- Development and production configuration, Docker Compose for PostgreSQL and a Dockerfile for the API.

### Changed

- Replaced the previous minimal project structure with the layered bounded-context structure.

### Documentation

- Added project overview and setup guide, architecture decision records, class diagram and API technical stories.
