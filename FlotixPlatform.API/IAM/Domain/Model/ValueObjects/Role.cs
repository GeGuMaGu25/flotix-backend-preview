using System.Text.Json.Serialization;

namespace FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

/// <summary>
///     Represents the role of a user in the platform.
/// </summary>
/// <remarks>
///     Serialized with the Spanish names used by the Flotix front end and persisted by member name.
/// </remarks>
public enum Role
{
    /// <summary>
    ///     A fleet owner that manages vehicles, drivers and purchases.
    /// </summary>
    [JsonStringEnumMemberName("Owner")]
    Owner,

    /// <summary>
    ///     A driver that operates vehicles and reports refuels and incidents.
    /// </summary>
    [JsonStringEnumMemberName("Driver")]
    Driver,

    /// <summary>
    ///     A mechanic that runs a workshop and attends maintenance requests.
    /// </summary>
    [JsonStringEnumMemberName("Mechanic")]
    Mechanic
}
