# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
