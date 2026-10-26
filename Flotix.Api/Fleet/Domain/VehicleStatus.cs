using System.Text.Json.Serialization;

namespace Flotix.Api.Fleet.Domain;

public enum VehicleStatus
{
    [JsonStringEnumMemberName("Disponible")] Available,
    [JsonStringEnumMemberName("En mantenimiento")] InMaintenance,
    [JsonStringEnumMemberName("Inactivo")] Inactive
}
