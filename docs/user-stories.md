# Flotix Platform REST API Technical Stories

## Overview
Este documento reproduce las historias técnicas (TS01 a TS31) del capítulo 3.1 del informe del proyecto, redactadas con el rol Developer para respaldar la implementación de los servicios del Backend, e indica el estado de implementación de cada una en este repositorio.

Ruta base de los recursos: `/api/v1/<recurso>` (convención de nombres kebab-case). Los valores de estado usan los nombres en español del informe.

| Estado | Significado |
|---|---|
| ✅ Implementada | Todos los escenarios de aceptación están implementados. |
| 🟡 Parcial | Parte de los escenarios está implementada; la nota indica lo que falta. |
| ⏳ Pendiente | Fuera del alcance del Sprint 3. |

## Resumen

| ID | Título | Epic | Estado |
|---|---|---|---|
| TS01 | Implementar registro y consulta de vehículos vía API | EP01 | ✅ Implementada (Sprint 3) |
| TS02 | Implementar ingesta de datos IoT en tiempo real | EP05 | ✅ Implementada (Sprint 3) |
| TS03 | Implementar flujo de solicitud de mantenimiento entre Dueño y Mecánica | EP03 | ✅ Implementada (Sprint 3) |
| TS04 | Implementar generación de reportes | EP08 | ✅ Implementada (Sprint 3) |
| TS05 | Implementar actualización y eliminación de vehículos vía API | EP01 | ✅ Implementada (Sprint 3) |
| TS06 | Implementar asignación de conductores a vehículos vía API | EP01 | ⏳ Pendiente |
| TS07 | Implementar consulta y búsqueda de conductores vía API | EP01 | 🟡 Parcial |
| TS08 | Implementar registro de recargas de combustible vía API | EP02 | 🟡 Parcial |
| TS09 | Implementar historial y cálculo de rendimiento de combustible | EP02 | ⏳ Pendiente |
| TS10 | Implementar detección de consumo anómalo | EP02 | ⏳ Pendiente |
| TS11 | Implementar resumen y eliminación de registros de combustible | EP02 | 🟡 Parcial |
| TS12 | Implementar creación y consulta de solicitudes de mantenimiento vía API | EP03 | 🟡 Parcial |
| TS13 | Implementar sincronización del estado del vehículo con el mantenimiento | EP03 | 🟡 Parcial |
| TS14 | Implementar consulta y búsqueda de talleres afiliados | EP03 | ⏳ Pendiente |
| TS15 | Implementar eliminación de solicitudes de mantenimiento vía API | EP03 | ✅ Implementada (Sprint 3) |
| TS16 | Implementar registro de incidencias y generación de notificación | EP04 | 🟡 Parcial |
| TS17 | Implementar ciclo de vida de incidencias | EP04 | ⏳ Pendiente |
| TS18 | Implementar consulta y eliminación de incidencias | EP04 | 🟡 Parcial |
| TS19 | Implementar vinculación de dispositivos IoT con vehículos | EP05 | ✅ Implementada (Sprint 3) |
| TS20 | Implementar consulta de posición e indicadores de monitoreo | EP05 | 🟡 Parcial |
| TS21 | Implementar generación de alertas de exceso de velocidad | EP06 | ⏳ Pendiente |
| TS22 | Implementar consulta de alertas y notificaciones | EP06 | 🟡 Parcial |
| TS23 | Implementar marcado de notificaciones como leídas | EP06 | 🟡 Parcial |
| TS24 | Implementar catálogo de dispositivos IoT vía API | EP07 | ⏳ Pendiente |
| TS25 | Implementar creación de pedidos y confirmación de pago | EP07 | 🟡 Parcial |
| TS26 | Implementar consulta del historial de pedidos | EP07 | ✅ Implementada |
| TS27 | Implementar avance de estado de pedidos | EP07 | 🟡 Parcial |
| TS28 | Implementar panel general de operación | EP08 | ⏳ Pendiente |
| TS29 | Implementar resumen consolidado y comparativo de rendimiento | EP08 | ⏳ Pendiente |
| TS30 | Implementar exportación e historial de reportes | EP08 | 🟡 Parcial |
| TS31 | Documentar el RESTful API con OpenAPI | EP01 a EP08 | ✅ Implementada (Sprint 3) |

## Technical Stories

### TS01 — Implementar registro y consulta de vehículos vía API
Como Developer, Quiero implementar los endpoints de creación y consulta de vehículos, Para que el Frontend pueda gestionar la flota.
**Relacionado con:** EP01
#### Criterios de aceptación:
- Escenario 1: Creación exitosa
    - Dado un conjunto de datos válido
    - Cuando se envía al servicio de registro
    - Entonces retorna el código 201 (Creado) con el recurso creado
- Escenario 2: Datos inválidos
    - Dado que los datos están incompletos
    - Cuando se procesan
    - Entonces retorna el código 400 (Solicitud incorrecta) con el detalle del error
#### Implementación en el backend
- **Estado:** ✅ Implementada en Sprint 3
- **Endpoints:** `POST /api/v1/vehicles`, `GET /api/v1/vehicles`, `GET /api/v1/vehicles/{id}`
- **Nota:** Los datos inválidos retornan 400 con el detalle del error localizado.
---
### TS02 — Implementar ingesta de datos IoT en tiempo real
Como Developer, Quiero implementar el servicio que recibe y procesa la telemetría enviada por los dispositivos IoT (GPS, odómetro, combustible), Para alimentar el monitoreo en tiempo real de la plataforma.
**Relacionado con:** EP05
#### Criterios de aceptación:
- Escenario 1: Ingesta exitosa
    - Dado que un dispositivo envía un paquete de telemetría válido
    - Cuando el servicio lo procesa
    - Entonces actualiza la posición y los indicadores del vehículo correspondiente
- Escenario 2: Paquete corrupto
    - Dado que el paquete recibido no cumple el esquema esperado
    - Cuando se valida
    - Entonces el servicio lo descarta y registra el evento en el registro de errores
#### Implementación en el backend
- **Estado:** ✅ Implementada en Sprint 3
- **Endpoints:** `POST /api/v1/telemetry-data` (202 Accepted), hub `/hubs/monitoring` (`VehicleLocationUpdated`)
- **Nota:** El IoT Telemetry Processor procesa los paquetes en segundo plano: almacena la lectura en `telemetry_data`, marca el dispositivo como conectado y publica la ubicación del vehículo. Los paquetes corruptos o de dispositivos sin vínculo se descartan y se registran en el registro de errores.
---
### TS03 — Implementar flujo de solicitud de mantenimiento entre Dueño y Mecánica
Como Developer, Quiero implementar los endpoints que gestionan el ciclo de vida de una solicitud de mantenimiento (creación, diagnóstico, presupuesto, aprobación y cierre), Para que Dueños y talleres puedan coordinar un servicio de principio a fin desde la plataforma.
**Relacionado con:** EP03
#### Criterios de aceptación:
- Escenario 1: Transición válida
    - Dada una solicitud en estado "Pendiente"
    - Cuando el taller registra un diagnóstico
    - Entonces el sistema la pasa al estado "Presupuestado"
- Escenario 2: Transición inválida
    - Dada una solicitud ya cerrada
    - Cuando se intenta modificar
    - Entonces el sistema retorna el código 409 (Conflicto)
#### Implementación en el backend
- **Estado:** ✅ Implementada en Sprint 3
- **Endpoints:** `POST /api/v1/maintenance-requests`, `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote`, `/budget-decision`, `/repair-start`, `/completion`
- **Nota:** Las transiciones inválidas o sobre solicitudes cerradas retornan 409 (Conflicto).
---
### TS04 — Implementar generación de reportes
Como Developer, Quiero implementar los endpoints de generación de reportes de combustible y mantenimiento, Para que Dueños y talleres puedan analizar su operación desde la plataforma.
**Relacionado con:** EP08
#### Criterios de aceptación:
- Escenario 1: Generación exitosa
    - Dado un rango de fechas y tipo de reporte válidos
    - Cuando se solicita
    - Entonces el sistema retorna el reporte correspondiente
- Escenario 2: Sin datos disponibles
    - Dado que no existen registros en el rango solicitado
    - Cuando se procesa
    - Entonces retorna un mensaje informativo sin error
#### Implementación en el backend
- **Estado:** ✅ Implementada en Sprint 3
- **Endpoints:** `POST /api/v1/reports`, `GET /api/v1/reports`
- **Nota:** Genera reportes de consumo de combustible y de costos de mantenimiento por rango de fechas; sin datos retorna un mensaje informativo sin error.
---
### TS05 — Implementar actualización y eliminación de vehículos vía API
Como Developer, Quiero implementar los endpoints de modificación y eliminación de vehículos, Para que el Frontend pueda mantener actualizada la información de la flota.
**Relacionado con:** EP01
#### Criterios de aceptación:
- Escenario 1: Actualización exitosa
    - Dado un vehículo existente y datos válidos
    - Cuando se envían al servicio de actualización
    - Entonces retorna el código 200 (OK) con el recurso actualizado
- Escenario 2: Eliminación exitosa
    - Dado un vehículo existente
    - Cuando se solicita su eliminación
    - Entonces retorna el código 204 (Sin contenido) y el vehículo deja de figurar en la consulta de la flota
- Escenario 3: Vehículo inexistente
    - Dado un identificador que no corresponde a ningún vehículo
    - Cuando se solicita su actualización o eliminación
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** ✅ Implementada en Sprint 3
- **Endpoints:** `PUT /api/v1/vehicles/{id}`, `DELETE /api/v1/vehicles/{id}`
- **Nota:** Un vehículo con registros relacionados (combustible, mantenimientos, incidencias, telemetría) no se elimina y retorna 409, para conservar la trazabilidad.
---
### TS06 — Implementar asignación de conductores a vehículos vía API
Como Developer, Quiero implementar los endpoints de asignación y desasignación de conductores, Para que la plataforma mantenga la relación entre cada unidad y su conductor.
**Relacionado con:** EP01
#### Criterios de aceptación:
- Escenario 1: Asignación exitosa
    - Dado un vehículo y un conductor registrados
    - Cuando se solicita la asignación
    - Entonces retorna el código 200 (OK) con la asignación registrada y el conductor figura con estado "En ruta"
- Escenario 2: Desasignación
    - Dado un vehículo con conductor asignado
    - Cuando se solicita retirar la asignación
    - Entonces retorna el código 200 (OK) y el vehículo figura con conductor "Sin asignar"
- Escenario 3: Conductor inexistente
    - Dado un identificador que no corresponde a ningún conductor registrado
    - Cuando se solicita la asignación
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
- **Nota:** Hoy el conductor se asigna con `PUT /api/v1/vehicles/{id}` (`driverId`); faltan los endpoints de asignación/desasignación y los estados "En ruta" / "Sin asignar".
---
### TS07 — Implementar consulta y búsqueda de conductores vía API
Como Developer, Quiero implementar el endpoint de consulta de conductores con filtro de búsqueda, Para que el Frontend pueda listar y localizar al personal de la flota.
**Relacionado con:** EP01
#### Criterios de aceptación:
- Escenario 1: Lista completa
    - Dado que existen conductores registrados
    - Cuando se solicita la lista sin filtro
    - Entonces retorna el código 200 (OK) con nombre, correo, licencia, vehículo asignado y estado de cada conductor, junto con el total, los activos y los que están en ruta
- Escenario 2: Búsqueda con resultados
    - Dado un texto de búsqueda
    - Cuando coincide con el nombre, correo o licencia de un conductor
    - Entonces retorna el código 200 (OK) solo con los conductores que coinciden
- Escenario 3: Búsqueda sin resultados
    - Dado un texto de búsqueda que no coincide con ningún conductor
    - Cuando se procesa
    - Entonces retorna el código 200 (OK) con una lista vacía
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `GET /api/v1/users?roleType=Driver`
- **Nota:** Falta la búsqueda por nombre, correo o licencia y los contadores.
---
### TS08 — Implementar registro de recargas de combustible vía API
Como Developer, Quiero implementar el endpoint de registro de recargas de combustible, Para que los Conductores puedan reportar el consumo real de cada unidad.
**Relacionado con:** EP02
#### Criterios de aceptación:
- Escenario 1: Registro exitoso
    - Dado un vehículo existente y litros, costo total, kilometraje y fecha válidos
    - Cuando se envían al servicio de registro
    - Entonces retorna el código 201 (Creado) con la recarga creada
- Escenario 2: Datos inválidos
    - Dado que los litros o el costo son menores o iguales a cero, o que faltan campos obligatorios
    - Cuando se procesan
    - Entonces retorna el código 400 (Solicitud incorrecta) con el detalle del error
- Escenario 3: Kilometraje inconsistente
    - Dado que el kilometraje es menor al de la última recarga del vehículo
    - Cuando se procesa
    - Entonces retorna el código 400 (Solicitud incorrecta) indicando la inconsistencia
- Escenario 4: Vehículo inexistente
    - Dado un identificador que no corresponde a ningún vehículo
    - Cuando se solicita el registro
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `POST /api/v1/fuel-records`
- **Nota:** Cubre los escenarios 1, 2 y 4; falta validar el kilometraje contra la última recarga (escenario 3).
---
### TS09 — Implementar historial y cálculo de rendimiento de combustible
Como Developer, Quiero implementar el endpoint que retorna el historial de recargas con su rendimiento en km/L, Para que el Dueño pueda analizar el consumo de cada unidad.
**Relacionado con:** EP02
#### Criterios de aceptación:
- Escenario 1: Rendimiento calculado
    - Dado un vehículo con al menos dos recargas registradas
    - Cuando se solicita el historial
    - Entonces retorna el código 200 (OK) con el rendimiento en km/L de cada recarga posterior a la primera, calculado con la diferencia de kilometraje entre recargas
- Escenario 2: Primera recarga
    - Dado que una recarga es la primera del vehículo
    - Cuando se solicita el historial
    - Entonces el rendimiento de esa recarga se retorna sin valor
- Escenario 3: Orden del historial
    - Dado un vehículo con recargas de varias fechas
    - Cuando se solicita el historial
    - Entonces retorna las recargas de la más reciente a la más antigua
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
---
### TS10 — Implementar detección de consumo anómalo
Como Developer, Quiero implementar la regla que identifica recargas con rendimiento inusualmente bajo, Para que el Dueño pueda investigar posibles fugas, fallas o malas prácticas.
**Relacionado con:** EP02
#### Criterios de aceptación:
- Escenario 1: Consumo anómalo
    - Dado que el rendimiento de una recarga es inferior al 70% del promedio histórico del vehículo
    - Cuando el servicio calcula el rendimiento
    - Entonces marca la recarga como consumo anómalo en la respuesta
- Escenario 2: Consumo normal
    - Dado que el rendimiento es igual o superior al 70% del promedio histórico
    - Cuando el servicio calcula el rendimiento
    - Entonces no marca la recarga como anómala
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
---
### TS11 — Implementar resumen y eliminación de registros de combustible
Como Developer, Quiero implementar los endpoints de resumen de combustible y de eliminación de registros con recálculo, Para que el costo de combustible de la flota se mantenga exacto.
**Relacionado con:** EP02
#### Criterios de aceptación:
- Escenario 1: Resumen disponible
    - Dado que existen recargas registradas
    - Cuando se solicita el resumen
    - Entonces retorna el código 200 (OK) con los litros cargados, el gasto total en soles y la cantidad de consumos anómalos
- Escenario 2: Eliminación con recálculo
    - Dado un registro de combustible existente
    - Cuando se solicita su eliminación
    - Entonces retorna el código 204 (Sin contenido) y recalcula el rendimiento y el resumen
- Escenario 3: Registro inexistente
    - Dado un identificador que no corresponde a ningún registro
    - Cuando se solicita su eliminación
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `DELETE /api/v1/fuel-records/{id}`
- **Nota:** Cubre la eliminación (escenarios 2 sin recálculo y 3); falta el resumen.
---
### TS12 — Implementar creación y consulta de solicitudes de mantenimiento vía API
Como Developer, Quiero implementar los endpoints de creación y consulta de solicitudes de mantenimiento, con opción de dirigirlas a un taller elegido, Para que el Dueño pueda iniciar y dar seguimiento a cada servicio.
**Relacionado con:** EP03
#### Criterios de aceptación:
- Escenario 1: Creación exitosa
    - Dado un vehículo existente y la descripción de la falla
    - Cuando se envían al servicio de creación
    - Entonces retorna el código 201 (Creado) con la solicitud en estado "Pendiente", asignada al taller cuando se indica uno
- Escenario 2: Taller inexistente
    - Dado un identificador que no corresponde a ningún taller afiliado
    - Cuando se envía la solicitud
    - Entonces retorna el código 404 (No encontrado)
- Escenario 3: Consulta de solicitudes
    - Dado que existen solicitudes
    - Cuando se solicita la lista
    - Entonces retorna el código 200 (OK) con vehículo, falla, taller, presupuesto y estado de cada una, junto con las solicitudes abiertas, en reparación y completadas
- Escenario 4: Sin solicitudes
    - Dado que no existen solicitudes
    - Cuando se solicita la lista
    - Entonces retorna el código 200 (OK) con una lista vacía y los contadores en cero
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `POST /api/v1/maintenance-requests`, `GET /api/v1/maintenance-requests`
- **Nota:** La solicitud se dirige a un mecánico (`mechanicId`) y retorna 404 si no existe; faltan los contadores de la consulta.
---
### TS13 — Implementar sincronización del estado del vehículo con el mantenimiento
Como Developer, Quiero implementar la actualización automática del estado del vehículo según el avance de la reparación, Para que la disponibilidad de la flota refleje los servicios en curso.
**Relacionado con:** EP03
#### Criterios de aceptación:
- Escenario 1: Inicio de reparación
    - Dada una solicitud en estado "Aprobado"
    - Cuando se registra el inicio de la reparación
    - Entonces la solicitud pasa a "En reparación" y el vehículo a "En mantenimiento" dentro de la misma operación
- Escenario 2: Finalización de reparación
    - Dada una solicitud en estado "En reparación"
    - Cuando se registra la finalización
    - Entonces la solicitud pasa a "Completado" y el vehículo a "Disponible" dentro de la misma operación
- Escenario 3: Transición no permitida
    - Dada una solicitud en estado "Pendiente"
    - Cuando se intenta iniciar la reparación
    - Entonces retorna el código 409 (Conflicto) y el estado del vehículo no cambia
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `POST /api/v1/maintenance-requests/{id}/repair-start`, `/completion`
- **Nota:** Cubre las transiciones de la solicitud y el escenario 3; falta sincronizar el estado del vehículo.
---
### TS14 — Implementar consulta y búsqueda de talleres afiliados
Como Developer, Quiero implementar el endpoint de consulta de talleres afiliados con filtro de búsqueda, Para que el Dueño pueda elegir el taller más adecuado para su vehículo.
**Relacionado con:** EP03
#### Criterios de aceptación:
- Escenario 1: Lista de talleres
    - Dado que existen talleres afiliados
    - Cuando se solicita la lista sin filtro
    - Entonces retorna el código 200 (OK) con nombre, ciudad, dirección, teléfono, calificación y especialidades de cada taller, junto con la cantidad de talleres disponibles
- Escenario 2: Búsqueda con resultados
    - Dado un texto de búsqueda
    - Cuando coincide con el nombre o la ciudad de un taller
    - Entonces retorna el código 200 (OK) solo con los talleres que coinciden
- Escenario 3: Búsqueda sin resultados
    - Dado un texto de búsqueda que no coincide con ningún taller
    - Cuando se procesa
    - Entonces retorna el código 200 (OK) con una lista vacía
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
- **Nota:** El diseño de base de datos del informe (4.8) no incluye una tabla de talleres.
---
### TS15 — Implementar eliminación de solicitudes de mantenimiento vía API
Como Developer, Quiero implementar el endpoint de eliminación de solicitudes de mantenimiento, Para que el Dueño pueda retirar solicitudes registradas por error.
**Relacionado con:** EP03
#### Criterios de aceptación:
- Escenario 1: Eliminación exitosa
    - Dada una solicitud existente
    - Cuando se solicita su eliminación
    - Entonces retorna el código 204 (Sin contenido) y la solicitud deja de figurar en la consulta
- Escenario 2: Solicitud inexistente
    - Dado un identificador que no corresponde a ninguna solicitud
    - Cuando se solicita su eliminación
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** ✅ Implementada en Sprint 3
- **Endpoints:** `DELETE /api/v1/maintenance-requests/{id}`
---
### TS16 — Implementar registro de incidencias y generación de notificación
Como Developer, Quiero implementar el endpoint de registro de incidencias que notifica al Dueño, Para que las fallas reportadas por los Conductores se atiendan a tiempo.
**Relacionado con:** EP04
#### Criterios de aceptación:
- Escenario 1: Registro exitoso
    - Dado un vehículo existente y la descripción del problema
    - Cuando se envían al servicio de registro
    - Entonces retorna el código 201 (Creado) con la incidencia en estado "Abierto" y genera la notificación para el Dueño de la flota
- Escenario 2: Datos incompletos
    - Dado que falta la descripción o el vehículo
    - Cuando se procesan
    - Entonces retorna el código 400 (Solicitud incorrecta) con el detalle del error
- Escenario 3: Vehículo inexistente
    - Dado un identificador que no corresponde a ningún vehículo
    - Cuando se solicita el registro
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `POST /api/v1/incidents`
- **Nota:** Cubre los escenarios de registro, datos incompletos y vehículo inexistente; falta generar la notificación al Dueño.
---
### TS17 — Implementar ciclo de vida de incidencias
Como Developer, Quiero implementar los endpoints que cambian el estado de una incidencia entre Abierto, En revisión y Resuelto, Para que el Dueño pueda dar seguimiento ordenado a cada problema.
**Relacionado con:** EP04
#### Criterios de aceptación:
- Escenario 1: Paso a revisión
    - Dada una incidencia en estado "Abierto"
    - Cuando se solicita iniciar su revisión
    - Entonces retorna el código 200 (OK) con la incidencia en estado "En revisión"
- Escenario 2: Resolución
    - Dada una incidencia en estado "En revisión"
    - Cuando se solicita marcarla como resuelta
    - Entonces retorna el código 200 (OK) con la incidencia en estado "Resuelto"
- Escenario 3: Transición inválida
    - Dada una incidencia en estado "Abierto"
    - Cuando se solicita marcarla como resuelta
    - Entonces retorna el código 409 (Conflicto) indicando que primero debe pasar a "En revisión"
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
- **Nota:** Hoy `PUT /api/v1/incidents/{id}` permite cambiar el estado sin validar el ciclo de vida.
---
### TS18 — Implementar consulta y eliminación de incidencias
Como Developer, Quiero implementar los endpoints de consulta y eliminación de incidencias, Para que el Dueño pueda revisar el estado de los problemas y retirar reportes duplicados o erróneos.
**Relacionado con:** EP04
#### Criterios de aceptación:
- Escenario 1: Consulta de incidencias
    - Dado que existen incidencias
    - Cuando se solicita la lista
    - Entonces retorna el código 200 (OK) con vehículo, descripción, fecha y estado de cada una, junto con el total, las abiertas y las resueltas
- Escenario 2: Eliminación exitosa
    - Dada una incidencia existente
    - Cuando se solicita su eliminación
    - Entonces retorna el código 204 (Sin contenido)
- Escenario 3: Incidencia inexistente
    - Dado un identificador que no corresponde a ninguna incidencia
    - Cuando se solicita su eliminación
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `GET /api/v1/incidents`, `DELETE /api/v1/incidents/{id}`
- **Nota:** Faltan los contadores de la consulta.
---
### TS19 — Implementar vinculación de dispositivos IoT con vehículos
Como Developer, Quiero implementar el endpoint que asocia un dispositivo IoT a un vehículo de la flota, Para que la telemetría recibida se atribuya a la unidad correcta.
**Relacionado con:** EP05
#### Criterios de aceptación:
- Escenario 1: Vinculación exitosa
    - Dado un dispositivo con identificador único y un vehículo existente
    - Cuando se solicita la vinculación
    - Entonces retorna el código 201 (Creado) con el vínculo registrado
- Escenario 2: Dispositivo ya vinculado
    - Dado un dispositivo asociado a otro vehículo
    - Cuando se solicita la vinculación
    - Entonces retorna el código 409 (Conflicto)
- Escenario 3: Telemetría de dispositivo no vinculado
    - Dado un paquete de telemetría de un dispositivo sin vínculo
    - Cuando el servicio de ingesta lo procesa
    - Entonces lo descarta y registra el evento en el registro de errores
#### Implementación en el backend
- **Estado:** ✅ Implementada en Sprint 3
- **Endpoints:** `POST /api/v1/iot-devices`
- **Nota:** Un vehículo admite un solo dispositivo y un número de serie un solo vínculo (409); la telemetría de dispositivos sin vínculo se descarta y se registra (TS02).
---
### TS20 — Implementar consulta de posición e indicadores de monitoreo
Como Developer, Quiero implementar el endpoint que retorna la última posición, la velocidad y los indicadores de monitoreo de la flota, Para que el Dueño supervise su operación en tiempo real.
**Relacionado con:** EP05
#### Criterios de aceptación:
- Escenario 1: Estado actual de la flota
    - Dado que existen dispositivos conectados con telemetría recibida
    - Cuando se solicita el estado de monitoreo
    - Entonces retorna el código 200 (OK) con la última posición y velocidad de cada vehículo, la cantidad de dispositivos conectados, la velocidad promedio y la cantidad de paquetes recibidos
- Escenario 2: Lectura más reciente
    - Dado que el servicio recibe un nuevo paquete de un dispositivo
    - Cuando se solicita nuevamente el estado de monitoreo
    - Entonces la respuesta incluye la lectura más reciente de ese dispositivo
- Escenario 3: Sin dispositivos conectados
    - Dado que no existen dispositivos conectados
    - Cuando se solicita el estado de monitoreo
    - Entonces retorna el código 200 (OK) con una lista vacía y los indicadores en cero
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `GET /api/v1/telemetry-data?deviceId={id}`, hub `/hubs/monitoring`
- **Nota:** Retorna las lecturas de la más reciente a la más antigua y publica cada nueva ubicación; faltan los indicadores consolidados.
---
### TS21 — Implementar generación de alertas de exceso de velocidad
Como Developer, Quiero implementar la regla que evalúa la velocidad de cada lectura de telemetría y genera alertas, Para que el Dueño pueda corregir conductas de riesgo.
**Relacionado con:** EP06
#### Criterios de aceptación:
- Escenario 1: Límite superado
    - Dado que un paquete de telemetría registra una velocidad mayor a 90 km/h
    - Cuando el servicio evalúa la lectura
    - Entonces registra una alerta de exceso de velocidad con la placa y la velocidad, y genera la notificación para el Dueño
- Escenario 2: Velocidad dentro del límite
    - Dado que la velocidad registrada es de 90 km/h o menos
    - Cuando el servicio evalúa la lectura
    - Entonces no registra alerta ni genera notificación
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
---
### TS22 — Implementar consulta de alertas y notificaciones
Como Developer, Quiero implementar los endpoints de consulta de alertas y notificaciones del Dueño, Para que pueda revisar los eventos críticos de su flota.
**Relacionado con:** EP06
#### Criterios de aceptación:
- Escenario 1: Alertas y notificaciones
    - Dado que existen alertas y notificaciones
    - Cuando se solicita la consulta
    - Entonces retorna el código 200 (OK) con el tipo, vehículo y fecha de cada alerta, el mensaje de cada notificación y la cantidad de alertas activas, notificaciones y notificaciones sin leer
- Escenario 2: Sin eventos
    - Dado que no existen alertas ni notificaciones
    - Cuando se solicita la consulta
    - Entonces retorna el código 200 (OK) con listas vacías y los contadores en cero
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `GET /api/v1/alerts`, `GET /api/v1/notifications?userId={id}`
- **Nota:** Faltan los contadores.
---
### TS23 — Implementar marcado de notificaciones como leídas
Como Developer, Quiero implementar el endpoint que actualiza el estado de lectura de una notificación, Para que el Dueño distinga las notificaciones que ya revisó.
**Relacionado con:** EP06
#### Criterios de aceptación:
- Escenario 1: Marcado exitoso
    - Dada una notificación sin leer
    - Cuando se solicita marcarla como leída
    - Entonces retorna el código 200 (OK) con la notificación leída y el contador de notificaciones sin leer disminuye en uno
- Escenario 2: Notificación ya leída
    - Dada una notificación que ya está leída
    - Cuando se solicita marcarla nuevamente
    - Entonces retorna el código 200 (OK) sin modificar el contador
- Escenario 3: Notificación inexistente
    - Dado un identificador que no corresponde a ninguna notificación
    - Cuando se solicita marcarla como leída
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `PUT /api/v1/notifications/{id}`
- **Nota:** Marca la notificación como leída y retorna 404 si no existe; falta el contador de no leídas.
---
### TS24 — Implementar catálogo de dispositivos IoT vía API
Como Developer, Quiero implementar los endpoints de consulta del catálogo de dispositivos IoT, Para que el Dueño pueda elegir el equipamiento de su flota.
**Relacionado con:** EP07
#### Criterios de aceptación:
- Escenario 1: Catálogo disponible
    - Dado que existen dispositivos en el catálogo
    - Cuando se solicita la lista
    - Entonces retorna el código 200 (OK) con el Rastreador GPS Flotix (S/ 149.90), el Odómetro digital Flotix (S/ 89.90) y el Sensor de combustible Flotix (S/ 119.90)
- Escenario 2: Producto inexistente
    - Dado un identificador que no corresponde a ningún dispositivo
    - Cuando se solicita su detalle
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
- **Nota:** El diseño de base de datos del informe (4.8) no incluye una tabla de catálogo.
---
### TS25 — Implementar creación de pedidos y confirmación de pago
Como Developer, Quiero implementar los endpoints de creación de pedidos de dispositivos IoT y de recepción de la confirmación de pago, Para que el Dueño pueda completar la compra de su equipamiento.
**Relacionado con:** EP07
#### Criterios de aceptación:
- Escenario 1: Creación exitosa
    - Dado un dispositivo existente y una cantidad entera mayor a cero
    - Cuando se envían al servicio de pedidos
    - Entonces retorna el código 201 (Creado) con el pedido en estado "Pendiente de pago" y un total igual al precio unitario por la cantidad
- Escenario 2: Cantidad inválida
    - Dado que la cantidad es menor o igual a cero
    - Cuando se procesa
    - Entonces retorna el código 400 (Solicitud incorrecta) con el detalle del error
- Escenario 3: Pago confirmado
    - Dado un pedido en estado "Pendiente de pago"
    - Cuando el servicio recibe la confirmación del pago
    - Entonces el pedido pasa automáticamente al estado "En preparación"
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `POST /api/v1/iot-orders`
- **Nota:** Crea el pedido en "Pendiente de pago" y valida la cantidad; el total por precio unitario depende del catálogo (TS24) y falta la confirmación de pago.
---
### TS26 — Implementar consulta del historial de pedidos
Como Developer, Quiero implementar el endpoint de consulta de pedidos de dispositivos IoT del Dueño, Para que pueda hacer seguimiento a sus compras.
**Relacionado con:** EP07
#### Criterios de aceptación:
- Escenario 1: Pedidos registrados
    - Dado que el Dueño realizó compras
    - Cuando se solicita el historial
    - Entonces retorna el código 200 (OK) con producto, cantidad, total, fecha y estado de cada pedido
- Escenario 2: Sin pedidos
    - Dado que el Dueño no realizó compras
    - Cuando se solicita el historial
    - Entonces retorna el código 200 (OK) con una lista vacía
#### Implementación en el backend
- **Estado:** ✅ Implementada
- **Endpoints:** `GET /api/v1/iot-orders?ownerId={id}`
---
### TS27 — Implementar avance de estado de pedidos
Como Developer, Quiero implementar el endpoint que avanza el estado de un pedido hasta su entrega, Para que el seguimiento de la compra se mantenga actualizado.
**Relacionado con:** EP07
#### Criterios de aceptación:
- Escenario 1: Avance de estado
    - Dado un pedido en estado "Pendiente de pago" o "En preparación"
    - Cuando se solicita avanzar su estado
    - Entonces retorna el código 200 (OK) con el pedido en el siguiente estado del flujo hasta "Entregado"
- Escenario 2: Pedido finalizado
    - Dado un pedido en estado "Entregado" o "Cancelado"
    - Cuando se solicita avanzar su estado
    - Entonces retorna el código 409 (Conflicto) y el estado no cambia
- Escenario 3: Pedido inexistente
    - Dado un identificador que no corresponde a ningún pedido
    - Cuando se solicita avanzar su estado
    - Entonces retorna el código 404 (No encontrado)
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `PUT /api/v1/iot-orders/{id}`
- **Nota:** Los pedidos entregados o cancelados retornan 409; falta el avance automático al siguiente estado.
---
### TS28 — Implementar panel general de operación
Como Developer, Quiero implementar el endpoint que consolida los indicadores operativos de la flota, Para que el Dueño identifique de inmediato lo que requiere su atención.
**Relacionado con:** EP08
#### Criterios de aceptación:
- Escenario 1: Resumen operativo
    - Dado que existen registros de la flota
    - Cuando se solicita el panel general
    - Entonces retorna el código 200 (OK) con los vehículos disponibles, los vehículos en mantenimiento, el gasto de combustible, las alertas abiertas, el estado de la flota y los mantenimientos pendientes
- Escenario 2: Incidencia urgente
    - Dado que existe al menos una incidencia en estado "Abierto"
    - Cuando se solicita el panel general
    - Entonces la respuesta marca esa incidencia como urgente
- Escenario 3: Sin mantenimientos pendientes
    - Dado que no existen solicitudes abiertas
    - Cuando se solicita el panel general
    - Entonces la respuesta indica cero mantenimientos pendientes
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
---
### TS29 — Implementar resumen consolidado y comparativo de rendimiento
Como Developer, Quiero implementar los endpoints del resumen consolidado de la flota y del comparativo de rendimiento por vehículo, Para que el Dueño conozca el costo total de su operación e identifique las unidades menos eficientes.
**Relacionado con:** EP08
#### Criterios de aceptación:
- Escenario 1: Datos consolidados
    - Dado que existen registros en la flota
    - Cuando se solicita el resumen consolidado
    - Entonces retorna el código 200 (OK) con la cantidad de vehículos, el gasto total en combustible, el gasto total en mantenimiento y los mantenimientos completados
- Escenario 2: Comparativo generado
    - Dado que existen vehículos con al menos dos recargas registradas
    - Cuando se solicita el comparativo
    - Entonces retorna el código 200 (OK) con la placa, el gasto total y el rendimiento promedio en km/L de cada vehículo
- Escenario 3: Historial insuficiente
    - Dado un vehículo con menos de dos recargas registradas
    - Cuando se genera el comparativo
    - Entonces el servicio excluye ese vehículo de la respuesta
#### Implementación en el backend
- **Estado:** ⏳ Pendiente
---
### TS30 — Implementar exportación e historial de reportes
Como Developer, Quiero implementar los endpoints de exportación de reportes de rendimiento y de consulta de los reportes exportados, Para que el Dueño pueda compartir, archivar y llevar control de sus reportes.
**Relacionado con:** EP08
#### Criterios de aceptación:
- Escenario 1: Exportación exitosa
    - Dado un tipo de reporte válido
    - Cuando se solicita la exportación
    - Entonces retorna el código 200 (OK) con un archivo que contiene el resumen consolidado y el detalle por vehículo, y registra la exportación con su tipo y fecha
- Escenario 2: Tipo no soportado
    - Dado un tipo de reporte que la plataforma no genera
    - Cuando se solicita la exportación
    - Entonces retorna el código 400 (Solicitud incorrecta)
- Escenario 3: Historial de exportaciones
    - Dado que el Dueño exportó reportes
    - Cuando se solicita el historial
    - Entonces retorna el código 200 (OK) con el tipo y la fecha de generación de cada reporte
#### Implementación en el backend
- **Estado:** 🟡 Parcial
- **Endpoints:** `GET /api/v1/reports`
- **Nota:** Cubre el historial de reportes generados (TS04); falta la exportación a archivo.
---
### TS31 — Documentar el RESTful API con OpenAPI
Como Developer, Quiero documentar los endpoints del Backend con la especificación OpenAPI mediante Scalar, Para que el equipo y el Frontend consuman la API sin ambigüedad.
**Relacionado con:** EP01 a EP08
#### Criterios de aceptación:
- Escenario 1: Documentación disponible
    - Dado que el servicio está desplegado
    - Cuando se solicita la especificación
    - Entonces retorna el código 200 (OK) con los endpoints, parámetros, esquemas y códigos de respuesta de cada bounded context
- Escenario 2: Documentación actualizada
    - Dado que un endpoint se agrega o se modifica
    - Cuando se genera la especificación
    - Entonces la especificación refleja el cambio
#### Implementación en el backend
- **Estado:** ✅ Implementada en Sprint 3
- **Endpoints:** `/openapi/v1.json`, `/scalar`
- **Nota:** La especificación OpenAPI se documenta con Scalar (sin Swagger) y se localiza en inglés y español.
---
