using FlotixPlatform.API.Incidents.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Incidents.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an incident.
/// </summary>
/// <param name="Id">The server-generated ID of the incident</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Description">The description of the incident.</param>
/// <param name="Status">The current status of the incident.</param>
/// <param name="ReportedAt">The moment the incident was reported.</param>
public record IncidentResource(
    int Id,
    int VehicleId,
    int DriverId,
    string Description,
    IncidentStatus Status,
    DateTimeOffset ReportedAt);
