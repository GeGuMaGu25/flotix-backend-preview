using System.Text.Json.Serialization;

namespace Flotix.Api.Fleet.Domain;

public enum IncidentStatus
{
    [JsonStringEnumMemberName("Abierto")] Open,
    [JsonStringEnumMemberName("En revisión")] InReview,
    [JsonStringEnumMemberName("Resuelto")] Resolved
}
