# Flotix Backend

Web API de Flotix en **.NET 10 (LTS)** con controllers, que reemplaza a la fake API de
`jsonserver/` manteniendo su contrato REST (`/api/v1/<recurso-en-kebab-case>`).

## Stack

| Pieza | Tecnología |
| --- | --- |
| API | ASP.NET Core 10 · controllers · rutas `[controller]` en kebab-case |
| Persistencia | EF Core 10 + Npgsql sobre PostgreSQL (Docker) |
| Documentación | OpenAPI (`/openapi/v1.json`) + Scalar (`/scalar`) |
| Tiempo real | SignalR (`/hubs/monitoring`, `/hubs/notifications`) |
| IAM | Middleware propio + Data Protection + `PasswordHasher` (desconectado por defecto) |

## Arquitectura

Monolito DDD organizado por **bounded context** (un solo proyecto, una carpeta y un esquema
PostgreSQL por contexto). Cada contexto contiene `Domain/` (agregados y reglas), `Persistence/`
(su mapeo EF) y `Web/` (controllers, requests y hubs).

| Bounded context | Recursos |
| --- | --- |
| `Iam` | `users`, `authentication/sign-in` |
| `Fleet` | `vehicles`, `fuel-records`, `incidents`, `fleet-preferences` |
| `Maintenance` | `workshops`, `maintenance-requests` |
| `Monitoring` | `iot-devices`, `telemetry-data`, `alerts` + hub `monitoring` |
| `Notifications` | `notifications`, `notification-preferences` + hub `notifications` |
| `Store` | `iot-catalog`, `iot-orders` |
| `Reporting` | `reports` |

`Shared/` es el kernel compartido. Patrones: Repository y Unit of Work (`IRepository<T>`,
`IUnitOfWork`), Template Method (`CrudController`), Observer (`OnCreatedAsync` → SignalR) y
Specification (filtros `?campo=valor` como expresiones). Las dependencias se inyectan por
abstracción.

Igual que json-server, cada colección admite `GET` (con filtros de igualdad, p. ej.
`?vehicleId=…&status=Abierto`), `GET /{id}`, `POST`, `PUT`, `PATCH` (merge patch) y `DELETE`.

## Ejecutar

```bash
docker compose up -d                       # PostgreSQL en localhost:5432
dotnet run --project Flotix.Api            # aplica las migraciones al iniciar
```

Documentación en <http://localhost:5080/scalar>.

En producción la cadena de conexión se define con la variable `ConnectionStrings__Flotix`
y los orígenes CORS con `Cors__Origins__0`, `Cors__Origins__1`, …

## IAM

`POST /api/v1/users` (registro) y `POST /api/v1/authentication/sign-in` (devuelve un token
bearer) siempre están activos. La protección de endpoints la realiza `IamMiddleware`, que está
**comentado** en `Program.cs`; al descomentarlo:

- todo endpoint sin `[AllowAnonymous]` exige `Authorization: Bearer <token>`;
- `[AllowRoles(...)]` restringe por rol (p. ej. `iot-orders` y `reports` solo para `Owner`);
- los hubs leen el token de `?access_token=` y las notificaciones se entregan solo a su destinatario.

## Migraciones

```bash
dotnet tool restore
dotnet ef migrations add <Nombre> --project Flotix.Api --output-dir Shared/Persistence/Migrations
```
