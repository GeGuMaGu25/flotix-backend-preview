# Flotix Platform REST API Technical Stories

## Overview
This document contains API-focused technical stories intended for frontend or mobile developers integrating with the Flotix Platform REST API.

Base route: `/api/v1/<resource>` (kebab-case route naming convention). Status values use the Spanish names of the fake API.

## Technical Stories

### TS001 — Sign up a user
As a frontend developer, I want to register a user with its role and profile so that I can implement the sign-up form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/authentication/sign-up` with a valid email, password, role and full name
    - When the email is not registered
    - Then the API responds with `201 Created` and returns the user without its password
- Scenario: Duplicate email
    - Given a request `POST /api/v1/authentication/sign-up` with a registered email
    - When the API detects the duplicate
    - Then the API responds with `409 Conflict`
- Scenario: Driver without license
    - Given a request to sign up a `Driver` without `licenseNumber`
    - When the domain rejects it
    - Then the API responds with `400 Bad Request`
---
### TS002 — Sign in
As a frontend developer, I want to exchange credentials for a bearer token so that I can authenticate the user.
#### Acceptance criteria:
- Scenario: Valid credentials
    - Given a request `POST /api/v1/authentication/sign-in` with valid credentials
    - When the password matches
    - Then the API responds with `200 OK` and returns `id`, `email`, `role`, `fullName` and `token`
- Scenario: Invalid credentials
    - Given a request with a wrong email or password
    - When the credentials do not match
    - Then the API responds with `401 Unauthorized`
---
### TS003 — Manage user profiles
As a frontend developer, I want to list users (optionally `?role=`), get, update the profile and delete users at `/api/v1/users`.
#### Acceptance criteria:
- Scenario: Profile updated
    - Given a request `PUT /api/v1/users/{id}` with a valid profile
    - When the user exists
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find the user
    - Then the API responds with `404 Not Found`
---
### TS004 — Get and update the fleet preferences
As a frontend developer, I want to read and change the fleet-wide thresholds at `/api/v1/fleet-preferences`.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/fleet-preferences` with positive thresholds
    - When the values are valid
    - Then the API responds with `200 OK`
---
### TS005 — Create a vehicle
As a frontend developer, I want to create a vehicle so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/vehicles` with `ownerId`, `driverId`, `plate`, `vehicleType`, `currentMileage`, `status`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the vehicle with its `id`
- Scenario: A vehicle with the same plate already exists
    - Given a request `POST /api/v1/vehicles` that breaks the rule
    - When the API rejects it
    - Then the API responds with `409`
---
### TS006 — List vehicles
As a frontend developer, I want to list vehicles, optionally filtered by `?ownerId=`, `?driverId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/vehicles` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/vehicles` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS007 — Retrieve a vehicle by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/vehicles/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS008 — Update a vehicle
As a frontend developer, I want to update a vehicle so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/vehicles/{id}` with `driverId`, `plate`, `vehicleType`, `currentMileage`, `status`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
- Scenario: A vehicle with the same plate already exists
    - Given a request that breaks the rule
    - When the API rejects it
    - Then the API responds with `409`
- Scenario: The mileage of a vehicle cannot decrease
    - Given a request that breaks the rule
    - When the API rejects it
    - Then the API responds with `400`
---
### TS009 — Delete a vehicle
As a frontend developer, I want to delete a vehicle so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/vehicles/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS010 — Create a fuel record
As a frontend developer, I want to create a fuel record so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/fuel-records` with `vehicleId`, `driverId`, `liters`, `totalCost`, `mileageAtRefuel`, `date`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the fuel record with its `id`
---
### TS011 — List fuel records
As a frontend developer, I want to list fuel records, optionally filtered by `?vehicleId=`, `?driverId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/fuel-records` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/fuel-records` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS012 — Retrieve a fuel record by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/fuel-records/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS013 — Update a fuel record
As a frontend developer, I want to update a fuel record so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/fuel-records/{id}` with `liters`, `totalCost`, `mileageAtRefuel`, `date`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS014 — Delete a fuel record
As a frontend developer, I want to delete a fuel record so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/fuel-records/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS015 — Create a workshop
As a frontend developer, I want to create a workshop so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/workshops` with `mechanicId`, `city`, `district`, `address`, `phone`, `rating`, `tags`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the workshop with its `id`
---
### TS016 — List workshops
As a frontend developer, I want to list workshops, optionally filtered by `?mechanicId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/workshops` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/workshops` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS017 — Retrieve a workshop by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/workshops/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS018 — Update a workshop
As a frontend developer, I want to update a workshop so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/workshops/{id}` with `city`, `district`, `address`, `phone`, `rating`, `tags`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS019 — Delete a workshop
As a frontend developer, I want to delete a workshop so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/workshops/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS020 — Create a maintenance request
As a frontend developer, I want to create a maintenance request so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/maintenance-requests` with `vehicleId`, `mechanicId`, `faultDescription`, `estimatedCost`, `status`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the maintenance request with its `id`
---
### TS021 — List maintenance requests
As a frontend developer, I want to list maintenance requests, optionally filtered by `?vehicleId=`, `?mechanicId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/maintenance-requests` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/maintenance-requests` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS022 — Retrieve a maintenance request by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/maintenance-requests/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS023 — Update a maintenance request
As a frontend developer, I want to update a maintenance request so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/maintenance-requests/{id}` with `mechanicId`, `faultDescription`, `estimatedCost`, `status`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
- Scenario: A completed or rejected maintenance request cannot change its status
    - Given a request that breaks the rule
    - When the API rejects it
    - Then the API responds with `409`
---
### TS024 — Delete a maintenance request
As a frontend developer, I want to delete a maintenance request so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/maintenance-requests/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS025 — Create an incident
As a frontend developer, I want to create an incident so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/incidents` with `vehicleId`, `driverId`, `description`, `status`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the incident with its `id`
---
### TS026 — List incidents
As a frontend developer, I want to list incidents, optionally filtered by `?vehicleId=`, `?driverId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/incidents` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/incidents` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS027 — Retrieve an incident by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/incidents/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS028 — Update an incident
As a frontend developer, I want to update an incident so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/incidents/{id}` with `description`, `status`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS029 — Delete an incident
As a frontend developer, I want to delete an incident so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/incidents/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS030 — Create an IoT device
As a frontend developer, I want to create an IoT device so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/iot-devices` with `vehicleId`, `serialNumber`, `connectionStatus`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the IoT device with its `id`
- Scenario: An IoT device with the same serial number already exists
    - Given a request `POST /api/v1/iot-devices` that breaks the rule
    - When the API rejects it
    - Then the API responds with `409`
---
### TS031 — List IoT devices
As a frontend developer, I want to list IoT devices, optionally filtered by `?vehicleId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/iot-devices` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/iot-devices` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS032 — Retrieve an IoT device by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/iot-devices/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS033 — Update an IoT device
As a frontend developer, I want to update an IoT device so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/iot-devices/{id}` with `vehicleId`, `connectionStatus`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS034 — Delete an IoT device
As a frontend developer, I want to delete an IoT device so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/iot-devices/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS035 — Create a telemetry record
As a frontend developer, I want to create a telemetry record so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/telemetry-data` with `deviceId`, `latitude`, `longitude`, `speed`, `timestamp`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the telemetry record with its `id`
---
### TS036 — List telemetry records
As a frontend developer, I want to list telemetry records, optionally filtered by `?deviceId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/telemetry-data` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/telemetry-data` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS037 — Retrieve a telemetry record by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/telemetry-data/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS038 — Receive telemetry records in real time
As a frontend developer, I want to listen to `TelemetryRecordReceived` on `/hubs/monitoring` so that I can update the UI without polling.
#### Acceptance criteria:
- Scenario: Pushed
    - Given a client connected to the hub
    - When a telemetry record is created
    - Then the client receives the created resource
---
### TS039 — Create an alert
As a frontend developer, I want to create an alert so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/alerts` with `vehicleId`, `alertType`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the alert with its `id`
---
### TS040 — List alerts
As a frontend developer, I want to list alerts, optionally filtered by `?vehicleId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/alerts` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/alerts` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS041 — Retrieve an alert by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/alerts/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS042 — Receive alerts in real time
As a frontend developer, I want to listen to `AlertActivated` on `/hubs/notifications` so that I can update the UI without polling.
#### Acceptance criteria:
- Scenario: Pushed
    - Given a client connected to the hub
    - When an alert is created
    - Then the client receives the created resource
---
### TS043 — Create a notification
As a frontend developer, I want to create a notification so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/notifications` with `userId`, `alertId`, `message`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the notification with its `id`
---
### TS044 — List notifications
As a frontend developer, I want to list notifications, optionally filtered by `?userId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/notifications` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/notifications` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS045 — Retrieve a notification by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/notifications/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS046 — Update a notification
As a frontend developer, I want to update a notification so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/notifications/{id}` with `isRead`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS047 — Delete a notification
As a frontend developer, I want to delete a notification so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/notifications/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS048 — Receive notifications in real time
As a frontend developer, I want to listen to `NotificationReceived` on `/hubs/notifications` so that I can update the UI without polling.
#### Acceptance criteria:
- Scenario: Pushed
    - Given a client connected to the hub
    - When a notification is created
    - Then the client receives the created resource only if it is its recipient (IAM enabled)
---
### TS049 — Create a notification preference
As a frontend developer, I want to create a notification preference so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/notification-preferences` with `userId`, `maintenance`, `speed`, `incidents`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the notification preference with its `id`
- Scenario: A notification preference with the same user ID already exists
    - Given a request `POST /api/v1/notification-preferences` that breaks the rule
    - When the API rejects it
    - Then the API responds with `409`
---
### TS050 — List notification preferences
As a frontend developer, I want to list notification preferences, optionally filtered by `?userId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/notification-preferences` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/notification-preferences` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS051 — Retrieve a notification preference by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/notification-preferences/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS052 — Update a notification preference
As a frontend developer, I want to update a notification preference so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/notification-preferences/{id}` with `maintenance`, `speed`, `incidents`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS053 — Delete a notification preference
As a frontend developer, I want to delete a notification preference so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/notification-preferences/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS054 — Create an IoT catalog item
As a frontend developer, I want to create an IoT catalog item so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/iot-catalog` with `sku`, `name`, `unitPrice`, `iconName`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the IoT catalog item with its `id`
- Scenario: An IoT catalog item with the same SKU already exists
    - Given a request `POST /api/v1/iot-catalog` that breaks the rule
    - When the API rejects it
    - Then the API responds with `409`
---
### TS055 — List IoT catalog items
As a frontend developer, I want to list IoT catalog items so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/iot-catalog` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/iot-catalog` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS056 — Retrieve an IoT catalog item by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/iot-catalog/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS057 — Update an IoT catalog item
As a frontend developer, I want to update an IoT catalog item so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/iot-catalog/{id}` with `name`, `unitPrice`, `iconName`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS058 — Delete an IoT catalog item
As a frontend developer, I want to delete an IoT catalog item so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/iot-catalog/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS059 — Create an IoT order
As a frontend developer, I want to create an IoT order so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/iot-orders` with `ownerId`, `sku`, `deviceQuantity`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the IoT order with its `id`
- Scenario: The SKU is not in the IoT catalog
    - Given a request `POST /api/v1/iot-orders` that breaks the rule
    - When the API rejects it
    - Then the API responds with `400`
---
### TS060 — List IoT orders
As a frontend developer, I want to list IoT orders, optionally filtered by `?ownerId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/iot-orders` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/iot-orders` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS061 — Retrieve an IoT order by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/iot-orders/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS062 — Update an IoT order
As a frontend developer, I want to update an IoT order so that I can implement its edit form.
#### Acceptance criteria:
- Scenario: Updated
    - Given a request `PUT /api/v1/iot-orders/{id}` with `status`
    - When the data is valid
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
- Scenario: A delivered or cancelled order cannot change its status
    - Given a request that breaks the rule
    - When the API rejects it
    - Then the API responds with `409`
---
### TS063 — Create a report
As a frontend developer, I want to create a report so that I can implement its registration form.
#### Acceptance criteria:
- Scenario: Created
    - Given a request `POST /api/v1/reports` with `ownerId`, `reportType`, `filterParams`
    - When the data is valid
    - Then the API responds with `201 Created` and returns the report with its `id`
---
### TS064 — List reports
As a frontend developer, I want to list reports, optionally filtered by `?ownerId=`, so that I can implement the listing view.
#### Acceptance criteria:
- Scenario: Returns items
    - Given a request `GET /api/v1/reports` is received
    - When the API finds matching items
    - Then the API responds with `200 OK` and an array
- Scenario: Returns no items
    - Given a request `GET /api/v1/reports` is received
    - When nothing matches
    - Then the API responds with `200 OK` and an empty array
---
### TS065 — Retrieve a report by id
As a frontend developer, I want to request it by `{id}` so that I can implement the details view.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/reports/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
### TS066 — Delete a report
As a frontend developer, I want to delete a report so that I can implement its removal.
#### Acceptance criteria:
- Scenario: Deleted
    - Given a request `DELETE /api/v1/reports/{id}` is received
    - When the resource exists
    - Then the API responds with `204 No Content`
- Scenario: Not found
    - Given a request for a non-existent `{id}`
    - When the API does not find it
    - Then the API responds with `404 Not Found`
---
