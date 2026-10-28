using System.ComponentModel.DataAnnotations;
using FlotixPlatform.API.Incidents.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Incidents.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update an incident.
/// </summary>
/// <param name="Description">The description of the incident. (required)</param>
/// <param name="Status">The current status of the incident.</param>
public record UpdateIncidentResource(
    [Required(ErrorMessage = "RequiredDescription")]
    [StringLength(500, ErrorMessage = "StringLengthField")]
    string Description,
    IncidentStatus Status);
