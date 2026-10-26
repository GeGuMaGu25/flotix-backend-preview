using System.Text.Json.Serialization;

namespace Flotix.Api.Maintenance.Domain;

public enum MaintenanceStatus
{
    [JsonStringEnumMemberName("Pendiente")] Pending,
    [JsonStringEnumMemberName("Presupuestado")] Quoted,
    [JsonStringEnumMemberName("Aprobado")] Approved,
    [JsonStringEnumMemberName("Rechazado")] Rejected,
    [JsonStringEnumMemberName("En reparación")] InRepair,
    [JsonStringEnumMemberName("Completado")] Completed
}
