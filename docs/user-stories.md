# Flotix Platform REST API Technical Stories

## Overview
This document contains API-focused technical stories intended for frontend or mobile developers integrating with the flotix-platform REST API.

Base resource route: `/api/v1/{resource}` (kebab-case route naming convention).

## Technical Stories

### TS001 — Sign up a user
As a frontend developer, I want to register a user with the `Owner`, `Driver` or `Mechanic` role through the API so that I can implement the registration feature in my application.
#### Acceptance criteria:
- Scenario: Successful sign-up
    - Given a request `POST /api/v1/authentication/sign-up` that includes required fields (`email`, `password`, `roleType`, `name`) is received
    - When the API validates the data, hashes the password and persists the new user successfully
    - Then the API responds with `201 Created` and returns the created user resource (`id`, `email`, `roleType`, `name`, `licenseNumber`, `licenseExpiration`, `companyName`, `workshopName`) without the password.
- Scenario: Validation error
    - Given a request `POST /api/v1/authentication/sign-up` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Driver without license
    - Given a request `POST /api/v1/authentication/sign-up` with `roleType` `Driver` and without `licenseNumber` or `licenseExpiration` is received
    - When the API validates the driver profile
    - Then the API responds with `400 Bad Request` and a localized error message.
- Scenario: Duplicate email
    - Given a request `POST /api/v1/authentication/sign-up` for an `email` that already exists is received
    - When the API detects the duplicate email before persistence or during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/authentication/sign-up` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS002 — Sign in a user
As a frontend developer, I want to sign in with my credentials through the API so that I can obtain a token to call the protected endpoints from my application.
#### Acceptance criteria:
- Scenario: Valid credentials
    - Given a request `POST /api/v1/authentication/sign-in` with a registered `email` and its `password` is received
    - When the API verifies the credentials
    - Then the API responds with `200 OK` and returns the authenticated user resource (`id`, `email`, `roleType`, `name`, `token`), where `token` is a JWT that includes the user id and role.
- Scenario: Invalid credentials
    - Given a request `POST /api/v1/authentication/sign-in` with an unknown `email` or a wrong `password` is received
    - When the API cannot verify the credentials
    - Then the API responds with `401 Unauthorized` and a localized error message.
- Scenario: Validation error
    - Given a request `POST /api/v1/authentication/sign-in` with missing required fields is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
---
### TS003 — List users by `roleType`
As a frontend developer, I want to request the API to list users, optionally filtered by `{roleType}`, so that I can implement the user listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more users
    - Given a request `GET /api/v1/users?roleType={roleType}` is received
    - When the API finds one or more matching user resources
    - Then the API responds with `200 OK` and returns a non-empty array of user resources, with each resource including `id`, `email`, `roleType`, `name`, `licenseNumber`, `licenseExpiration`, `companyName`, `workshopName`.
- Scenario: Returns no users
    - Given a request `GET /api/v1/users?roleType={roleType}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS004 — Retrieve an user by id
As a frontend developer, I want to request an user by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/users/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the user resource (`id`, `email`, `roleType`, `name`, `licenseNumber`, `licenseExpiration`, `companyName`, `workshopName`).
- Scenario: Not found
    - Given a request `GET /api/v1/users/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS005 — Update an user
As a frontend developer, I want to update an user through the API so that I can implement the user edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/users/{id}` that includes required fields (`name`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `email`, `roleType`, `name`, `licenseNumber`, `licenseExpiration`, `companyName`, `workshopName`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/users/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/users/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Driver without license
    - Given a request `PUT /api/v1/users/{id}` for a driver without `licenseNumber` or `licenseExpiration` is received
    - When the API validates the driver profile
    - Then the API responds with `400 Bad Request` and a localized error message.
---
### TS006 — Delete an user
As a frontend developer, I want to delete an user through the API so that I can implement the user removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/users/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/users/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: User with related records
    - Given a request `DELETE /api/v1/users/{id}` for an user that still has vehicles, fuel records, incidents, maintenance requests, notifications, IoT orders or reports is received
    - When the API detects the related records during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message; the resource is not removed.
---
### TS007 — List vehicles by `ownerId` or `driverId`
As a frontend developer, I want to request the API to list vehicles, optionally filtered by `{ownerId}` or `{driverId}`, so that I can implement the vehicle listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more vehicles
    - Given a request `GET /api/v1/vehicles?ownerId={ownerId}&driverId={driverId}` is received
    - When the API finds one or more matching vehicle resources
    - Then the API responds with `200 OK` and returns a non-empty array of vehicle resources, with each resource including `id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`.
- Scenario: Returns no vehicles
    - Given a request `GET /api/v1/vehicles?ownerId={ownerId}&driverId={driverId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS008 — Retrieve a vehicle by id
As a frontend developer, I want to request a vehicle by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/vehicles/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the vehicle resource (`id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`).
- Scenario: Not found
    - Given a request `GET /api/v1/vehicles/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS009 — Register a vehicle
As a frontend developer, I want to register a vehicle of my fleet through the API so that I can implement the vehicle creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/vehicles` that includes required fields (`ownerId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`).
- Scenario: Validation error
    - Given a request `POST /api/v1/vehicles` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Duplicate license plate
    - Given a request `POST /api/v1/vehicles` for a `licensePlate` that already exists is received
    - When the API detects the duplicate license plate before persistence or during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/vehicles` with a `ownerId` or `driverId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/vehicles` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS010 — Update a vehicle
As a frontend developer, I want to update a vehicle through the API so that I can implement the vehicle edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/vehicles/{id}` that includes required fields (`licensePlate`, `vehicleType`, `currentMileage`, `status`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/vehicles/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/vehicles/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Mileage decrease
    - Given a request `PUT /api/v1/vehicles/{id}` with a `currentMileage` lower than the stored one is received
    - When the API applies the vehicle rules
    - Then the API responds with `400 Bad Request` and a localized error message.
- Scenario: Duplicate license plate
    - Given a request `PUT /api/v1/vehicles/{id}` with a `licensePlate` of another vehicle is received
    - When the API detects the duplicate license plate
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Referenced resource not found
    - Given a request `PUT /api/v1/vehicles/{id}` with a `driverId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS011 — Delete a vehicle
As a frontend developer, I want to delete a vehicle through the API so that I can implement the vehicle removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/vehicles/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/vehicles/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Vehicle with related records
    - Given a request `DELETE /api/v1/vehicles/{id}` for a vehicle that still has fuel records, maintenance requests, incidents, an IoT device or alerts is received
    - When the API detects the related records during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message; the resource is not removed.
---
### TS012 — List fuel records by `vehicleId` or `driverId`
As a frontend developer, I want to request the API to list fuel records, optionally filtered by `{vehicleId}` or `{driverId}`, so that I can implement the fuel record listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more fuel records
    - Given a request `GET /api/v1/fuel-records?vehicleId={vehicleId}&driverId={driverId}` is received
    - When the API finds one or more matching fuel record resources
    - Then the API responds with `200 OK` and returns a non-empty array of fuel record resources, with each resource including `id`, `vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`.
- Scenario: Returns no fuel records
    - Given a request `GET /api/v1/fuel-records?vehicleId={vehicleId}&driverId={driverId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS013 — Retrieve a fuel record by id
As a frontend developer, I want to request a fuel record by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/fuel-records/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the fuel record resource (`id`, `vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/fuel-records/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS014 — Register a fuel record
As a frontend developer, I want to register a refuel of a vehicle through the API so that I can implement the fuel record creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/fuel-records` that includes required fields (`vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`).
- Scenario: Validation error
    - Given a request `POST /api/v1/fuel-records` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/fuel-records` with a `vehicleId` or `driverId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/fuel-records` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS015 — Update a fuel record
As a frontend developer, I want to update a fuel record through the API so that I can implement the fuel record edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/fuel-records/{id}` that includes required fields (`liters`, `cost`, `mileageAtRefill`, `recordedDate`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/fuel-records/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/fuel-records/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS016 — Delete a fuel record
As a frontend developer, I want to delete a fuel record through the API so that I can implement the fuel record removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/fuel-records/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/fuel-records/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS017 — List maintenance requests by `vehicleId` or `mechanicId`
As a frontend developer, I want to request the API to list maintenance requests, optionally filtered by `{vehicleId}` or `{mechanicId}`, so that I can implement the maintenance request listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more maintenance requests
    - Given a request `GET /api/v1/maintenance-requests?vehicleId={vehicleId}&mechanicId={mechanicId}` is received
    - When the API finds one or more matching maintenance request resources
    - Then the API responds with `200 OK` and returns a non-empty array of maintenance request resources, with each resource including `id`, `vehicleId`, `mechanicId`, `description`, `estimatedCost`, `status`, `requestDate`.
- Scenario: Returns no maintenance requests
    - Given a request `GET /api/v1/maintenance-requests?vehicleId={vehicleId}&mechanicId={mechanicId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS018 — Retrieve a maintenance request by id
As a frontend developer, I want to request a maintenance request by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/maintenance-requests/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the maintenance request resource (`id`, `vehicleId`, `mechanicId`, `description`, `estimatedCost`, `status`, `requestDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/maintenance-requests/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS019 — Submit a maintenance request
As a frontend developer, I want to submit a maintenance request of a vehicle to a mechanic through the API so that I can implement the maintenance request creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/maintenance-requests` that includes required fields (`vehicleId`, `mechanicId`, `description`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `mechanicId`, `description`, `estimatedCost`, `status`, `requestDate`).
- Scenario: Validation error
    - Given a request `POST /api/v1/maintenance-requests` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/maintenance-requests` with a `vehicleId` or `mechanicId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/maintenance-requests` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS020 — Register the diagnostic and quote of a maintenance request
As a frontend developer, I want to register the diagnostic and estimated cost of a maintenance request so that I can implement the mechanic quotation feature in my application.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` with `estimatedCost` for a maintenance request in `Pendiente` status is received
    - When the API applies the transition to the maintenance request
    - Then the API responds with `200 OK` and returns the maintenance request resource with status `Presupuestado`.
- Scenario: Validation error
    - Given a request `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` with a negative or missing `estimatedCost` is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` for a maintenance request that is not in `Pendiente` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS021 — Approve or reject the budget of a maintenance request
As a frontend developer, I want to approve or reject the quoted budget of a maintenance request so that I can implement the owner budget decision feature in my application.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/budget-decision` with `isApproved` for a maintenance request in `Presupuestado` status is received
    - When the API applies the transition to the maintenance request
    - Then the API responds with `200 OK` and returns the maintenance request resource with status `Aprobado` or `Rechazado`.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/budget-decision` for a maintenance request that is not in `Presupuestado` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/maintenance-requests/{id}/budget-decision` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS022 — Start the repair of a maintenance request
As a frontend developer, I want to start the repair of an approved maintenance request so that I can implement the repair tracking feature in my application.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/repair-start` for a maintenance request in `Aprobado` status is received
    - When the API applies the transition to the maintenance request
    - Then the API responds with `200 OK` and returns the maintenance request resource with status `En reparación`.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/repair-start` for a maintenance request that is not in `Aprobado` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/maintenance-requests/{id}/repair-start` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS023 — Complete a maintenance request
As a frontend developer, I want to complete a maintenance request under repair so that I can implement the repair completion feature in my application.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/completion` for a maintenance request in `En reparación` status is received
    - When the API applies the transition to the maintenance request
    - Then the API responds with `200 OK` and returns the maintenance request resource with status `Completado`.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/completion` for a maintenance request that is not in `En reparación` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/maintenance-requests/{id}/completion` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS024 — Delete a maintenance request
As a frontend developer, I want to delete a maintenance request through the API so that I can implement the maintenance request removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/maintenance-requests/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/maintenance-requests/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS025 — List incidents by `vehicleId` or `driverId`
As a frontend developer, I want to request the API to list incidents, optionally filtered by `{vehicleId}` or `{driverId}`, so that I can implement the incident listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more incidents
    - Given a request `GET /api/v1/incidents?vehicleId={vehicleId}&driverId={driverId}` is received
    - When the API finds one or more matching incident resources
    - Then the API responds with `200 OK` and returns a non-empty array of incident resources, with each resource including `id`, `vehicleId`, `driverId`, `description`, `status`, `reportedDate`.
- Scenario: Returns no incidents
    - Given a request `GET /api/v1/incidents?vehicleId={vehicleId}&driverId={driverId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS026 — Retrieve an incident by id
As a frontend developer, I want to request an incident by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/incidents/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the incident resource (`id`, `vehicleId`, `driverId`, `description`, `status`, `reportedDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/incidents/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS027 — Report an incident
As a frontend developer, I want to report an incident of a vehicle through the API so that I can implement the incident creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/incidents` that includes required fields (`vehicleId`, `driverId`, `description`, `status`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `driverId`, `description`, `status`, `reportedDate`).
- Scenario: Validation error
    - Given a request `POST /api/v1/incidents` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/incidents` with a `vehicleId` or `driverId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/incidents` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS028 — Update an incident
As a frontend developer, I want to update an incident through the API so that I can implement the incident edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/incidents/{id}` that includes required fields (`description`, `status`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `vehicleId`, `driverId`, `description`, `status`, `reportedDate`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/incidents/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/incidents/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS029 — Delete an incident
As a frontend developer, I want to delete an incident through the API so that I can implement the incident removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/incidents/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/incidents/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS030 — List IoT devices by `vehicleId`
As a frontend developer, I want to request the API to list IoT devices, optionally filtered by `{vehicleId}`, so that I can implement the IoT device listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more IoT devices
    - Given a request `GET /api/v1/iot-devices?vehicleId={vehicleId}` is received
    - When the API finds one or more matching IoT device resources
    - Then the API responds with `200 OK` and returns a non-empty array of IoT device resources, with each resource including `id`, `vehicleId`, `serialNumber`, `isConnected`.
- Scenario: Returns no IoT devices
    - Given a request `GET /api/v1/iot-devices?vehicleId={vehicleId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS031 — Retrieve an IoT device by id
As a frontend developer, I want to request an IoT device by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/iot-devices/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the IoT device resource (`id`, `vehicleId`, `serialNumber`, `isConnected`).
- Scenario: Not found
    - Given a request `GET /api/v1/iot-devices/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS032 — Link an IoT device to a vehicle
As a frontend developer, I want to link an IoT device to a vehicle through the API so that I can implement the IoT device creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/iot-devices` that includes required fields (`vehicleId`, `serialNumber`, `isConnected`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `serialNumber`, `isConnected`).
- Scenario: Validation error
    - Given a request `POST /api/v1/iot-devices` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Vehicle already equipped
    - Given a request `POST /api/v1/iot-devices` for a `vehicleId` that already has an IoT device is received
    - When the API detects the duplicate vehicle before persistence or during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Duplicate serial number
    - Given a request `POST /api/v1/iot-devices` for a `serialNumber` that already exists is received
    - When the API detects the duplicate serial number before persistence or during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/iot-devices` with a `vehicleId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/iot-devices` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS033 — Update an IoT device
As a frontend developer, I want to update an IoT device through the API so that I can implement the IoT device edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/iot-devices/{id}` that includes required fields (`vehicleId`, `isConnected`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `vehicleId`, `serialNumber`, `isConnected`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/iot-devices/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/iot-devices/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Vehicle already equipped
    - Given a request `PUT /api/v1/iot-devices/{id}` with a `vehicleId` that already has another IoT device is received
    - When the API detects the duplicate vehicle
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Referenced resource not found
    - Given a request `PUT /api/v1/iot-devices/{id}` with a `vehicleId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS034 — Delete an IoT device
As a frontend developer, I want to delete an IoT device through the API so that I can implement the IoT device removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/iot-devices/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/iot-devices/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: IoT device with related records
    - Given a request `DELETE /api/v1/iot-devices/{id}` for an IoT device that still has telemetry data is received
    - When the API detects the related records during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message; the resource is not removed.
---
### TS035 — Receive IoT telemetry data
As a frontend developer, I want to send the telemetry packets of an IoT device to the API so that I can feed the real-time monitoring of the fleet.
#### Acceptance criteria:
- Scenario: Packet accepted
    - Given a request `POST /api/v1/telemetry-data` that includes required fields (`deviceId`, `latitude`, `longitude`, `speed`, `recordedAt`) is received
    - When the API validates the packet and queues it for processing
    - Then the API responds with `202 Accepted` without a body.
- Scenario: Packet processed
    - Given a queued telemetry packet of an IoT device linked to a vehicle
    - When the IoT Telemetry Processor processes the packet
    - Then the API stores the reading, marks the IoT device as connected and sends `VehicleLocationUpdated` (`vehicleId`, `deviceId`, `latitude`, `longitude`, `speed`, `recordedAt`) to the clients of `/hubs/monitoring`.
- Scenario: Corrupt packet
    - Given a request `POST /api/v1/telemetry-data` with missing fields or out-of-range coordinates or speed is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request`, discards the packet and writes it to the error log.
- Scenario: Unlinked device
    - Given a queued telemetry packet of a `deviceId` that is not linked to a vehicle
    - When the IoT Telemetry Processor processes the packet
    - Then the API discards the packet and writes it to the error log.
---
### TS036 — List telemetry data records by `deviceId`
As a frontend developer, I want to request the API to list telemetry data records, optionally filtered by `{deviceId}`, so that I can implement the telemetry data record listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more telemetry data records
    - Given a request `GET /api/v1/telemetry-data?deviceId={deviceId}` is received
    - When the API finds one or more matching telemetry data record resources
    - Then the API responds with `200 OK` and returns a non-empty array of telemetry data record resources ordered from the most recent to the oldest `recordedAt`, with each resource including `id`, `deviceId`, `latitude`, `longitude`, `speed`, `recordedAt`.
- Scenario: Returns no telemetry data records
    - Given a request `GET /api/v1/telemetry-data?deviceId={deviceId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS037 — Retrieve a telemetry data record by id
As a frontend developer, I want to request a telemetry data record by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/telemetry-data/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the telemetry data record resource (`id`, `deviceId`, `latitude`, `longitude`, `speed`, `recordedAt`).
- Scenario: Not found
    - Given a request `GET /api/v1/telemetry-data/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS038 — List alerts by `vehicleId`
As a frontend developer, I want to request the API to list alerts, optionally filtered by `{vehicleId}`, so that I can implement the alert listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more alerts
    - Given a request `GET /api/v1/alerts?vehicleId={vehicleId}` is received
    - When the API finds one or more matching alert resources
    - Then the API responds with `200 OK` and returns a non-empty array of alert resources, with each resource including `id`, `vehicleId`, `alertType`, `triggerDate`.
- Scenario: Returns no alerts
    - Given a request `GET /api/v1/alerts?vehicleId={vehicleId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS039 — Retrieve an alert by id
As a frontend developer, I want to request an alert by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/alerts/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the alert resource (`id`, `vehicleId`, `alertType`, `triggerDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/alerts/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS040 — Create an alert
As a frontend developer, I want to create an alert of a vehicle through the API so that I can implement the alert creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/alerts` that includes required fields (`vehicleId`, `alertType`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `alertType`, `triggerDate`).
- Scenario: Validation error
    - Given a request `POST /api/v1/alerts` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/alerts` with a `vehicleId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/alerts` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS041 — List notifications by `userId`
As a frontend developer, I want to request the API to list notifications, optionally filtered by `{userId}`, so that I can implement the notification listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more notifications
    - Given a request `GET /api/v1/notifications?userId={userId}` is received
    - When the API finds one or more matching notification resources
    - Then the API responds with `200 OK` and returns a non-empty array of notification resources, with each resource including `id`, `userId`, `alertId`, `message`, `isRead`, `sentDate`.
- Scenario: Returns no notifications
    - Given a request `GET /api/v1/notifications?userId={userId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS042 — Retrieve a notification by id
As a frontend developer, I want to request a notification by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/notifications/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the notification resource (`id`, `userId`, `alertId`, `message`, `isRead`, `sentDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/notifications/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS043 — Create a notification
As a frontend developer, I want to create a notification for a user through the API so that I can implement the notification creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/notifications` that includes required fields (`userId`, `message`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `userId`, `alertId`, `message`, `isRead`, `sentDate`).
- Scenario: Validation error
    - Given a request `POST /api/v1/notifications` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/notifications` with a `userId` or `alertId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/notifications` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS044 — Update a notification
As a frontend developer, I want to update a notification through the API so that I can implement the notification edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/notifications/{id}` that includes required fields (`isRead`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `userId`, `alertId`, `message`, `isRead`, `sentDate`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/notifications/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/notifications/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS045 — Delete a notification
As a frontend developer, I want to delete a notification through the API so that I can implement the notification removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/notifications/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/notifications/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS046 — Receive alerts and notifications in real time
As a frontend developer, I want to connect to the notifications hub so that I can show new alerts and notifications in my application without polling.
#### Acceptance criteria:
- Scenario: Alert created
    - Given a client connected to `/hubs/notifications`
    - When an alert is created
    - Then the API sends `AlertActivated` with the alert resource to the connected clients.
- Scenario: Notification created
    - Given a client connected to `/hubs/notifications` as the recipient user
    - When a notification is created
    - Then the API sends `NotificationReceived` with the notification resource only to the recipient user.
---
### TS047 — List IoT orders by `ownerId`
As a frontend developer, I want to request the API to list IoT orders, optionally filtered by `{ownerId}`, so that I can implement the IoT order listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more IoT orders
    - Given a request `GET /api/v1/iot-orders?ownerId={ownerId}` is received
    - When the API finds one or more matching IoT order resources
    - Then the API responds with `200 OK` and returns a non-empty array of IoT order resources, with each resource including `id`, `ownerId`, `quantity`, `totalAmount`, `status`, `orderDate`.
- Scenario: Returns no IoT orders
    - Given a request `GET /api/v1/iot-orders?ownerId={ownerId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS048 — Retrieve an IoT order by id
As a frontend developer, I want to request an IoT order by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/iot-orders/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the IoT order resource (`id`, `ownerId`, `quantity`, `totalAmount`, `status`, `orderDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/iot-orders/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS049 — Place an IoT order
As a frontend developer, I want to place an order of IoT devices through the API so that I can implement the IoT order creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/iot-orders` that includes required fields (`ownerId`, `quantity`, `totalAmount`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `ownerId`, `quantity`, `totalAmount`, `status`, `orderDate`).
- Scenario: Validation error
    - Given a request `POST /api/v1/iot-orders` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/iot-orders` with a `ownerId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/iot-orders` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS050 — Update an IoT order
As a frontend developer, I want to update an IoT order through the API so that I can implement the IoT order edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/iot-orders/{id}` that includes required fields (`status`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `ownerId`, `quantity`, `totalAmount`, `status`, `orderDate`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/iot-orders/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/iot-orders/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Closed order
    - Given a request `PUT /api/v1/iot-orders/{id}` for an order in `Entregado` or `Cancelado` status is received
    - When the API applies the order rules
    - Then the API responds with `409 Conflict` and a localized error message.
---
### TS051 — Generate a report
As a frontend developer, I want to generate a fuel consumption or maintenance cost report of my fleet for a date range so that I can implement the reports feature in my application.
#### Acceptance criteria:
- Scenario: Report with records
    - Given a request `POST /api/v1/reports` that includes required fields (`ownerId`, `reportType`, `from`, `to`) is received
    - When the API finds fuel records or maintenance requests of the owner vehicles within the period
    - Then the API responds with `201 Created` and returns the generated report (`id`, `ownerId`, `reportType`, `generatedDate`, `filterParams`, `lines`, `totalRecords`, `totalQuantity`, `totalCost`), with one line per vehicle.
- Scenario: Report without records
    - Given a request `POST /api/v1/reports` for a period without records is received
    - When the API finds no records
    - Then the API responds with `201 Created`, an empty `lines` array and a localized informative `message`.
- Scenario: Validation error
    - Given a request `POST /api/v1/reports` with an unsupported `reportType` or `from` after `to` is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/reports` with a `ownerId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/reports` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---
### TS052 — List reports by `ownerId`
As a frontend developer, I want to request the API to list reports, optionally filtered by `{ownerId}`, so that I can implement the report listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more reports
    - Given a request `GET /api/v1/reports?ownerId={ownerId}` is received
    - When the API finds one or more matching report resources
    - Then the API responds with `200 OK` and returns a non-empty array of report resources, with each resource including `id`, `ownerId`, `reportType`, `generatedDate`, `filterParams`.
- Scenario: Returns no reports
    - Given a request `GET /api/v1/reports?ownerId={ownerId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---
### TS053 — Retrieve a report by id
As a frontend developer, I want to request a report by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/reports/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the report resource (`id`, `ownerId`, `reportType`, `generatedDate`, `filterParams`).
- Scenario: Not found
    - Given a request `GET /api/v1/reports/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---
### TS054 — Delete a report
As a frontend developer, I want to delete a report through the API so that I can implement the report removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/reports/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/reports/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---
### TS055 — Read the API description in my language
As a frontend developer, I want the API description document to be served in my preferred language so that I can understand the endpoints and their responses without translating them.
#### Acceptance criteria:
- Scenario: Supported language requested
    - Given a request `GET /openapi/v1.json` with an `Accept-Language` header for a supported culture (`en`, `en-US`, `es`, `es-PE`) is received
    - When the API builds the description document
    - Then the API responds with `200 OK` and the operation summaries, descriptions, response descriptions and resource property descriptions are in that language.
- Scenario: Unsupported or missing language
    - Given a request `GET /openapi/v1.json` without an `Accept-Language` header, or with an unsupported one, is received
    - When the API builds the description document
    - Then the API responds with `200 OK` and the texts are in the default language.
