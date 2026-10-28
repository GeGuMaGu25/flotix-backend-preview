using System.Text.Json.Serialization;

namespace FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;

/// <summary>
///     Represents the connection status of an IoT device.
/// </summary>
/// <remarks>
///     Serialized with the Spanish names used by the Flotix front end and persisted by member name.
/// </remarks>
public enum ConnectionStatus
{
    /// <summary>
    ///     The device is online.
    /// </summary>
    [JsonStringEnumMemberName("Conectado")]
    Connected,

    /// <summary>
    ///     The device is offline.
    /// </summary>
    [JsonStringEnumMemberName("Desconectado")]
    Disconnected
}
