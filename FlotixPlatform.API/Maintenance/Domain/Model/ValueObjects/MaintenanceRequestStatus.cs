using System.Text.Json.Serialization;

namespace FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of a maintenance request.
/// </summary>
/// <remarks>
///     Serialized with the Spanish names used by the Flotix front end and persisted by member name.
/// </remarks>
public enum MaintenanceRequestStatus
{
    /// <summary>
    ///     The request is waiting for a quote.
    /// </summary>
    [JsonStringEnumMemberName("Pendiente")]
    Pending,

    /// <summary>
    ///     The request has been quoted.
    /// </summary>
    [JsonStringEnumMemberName("Presupuestado")]
    Quoted,

    /// <summary>
    ///     The quote has been approved.
    /// </summary>
    [JsonStringEnumMemberName("Aprobado")]
    Approved,

    /// <summary>
    ///     The quote has been rejected.
    /// </summary>
    [JsonStringEnumMemberName("Rechazado")]
    Rejected,

    /// <summary>
    ///     The vehicle is being repaired.
    /// </summary>
    [JsonStringEnumMemberName("En reparación")]
    InRepair,

    /// <summary>
    ///     The repair has been completed.
    /// </summary>
    [JsonStringEnumMemberName("Completado")]
    Completed
}
