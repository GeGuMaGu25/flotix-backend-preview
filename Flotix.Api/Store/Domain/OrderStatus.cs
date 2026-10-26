using System.Text.Json.Serialization;

namespace Flotix.Api.Store.Domain;

public enum OrderStatus
{
    [JsonStringEnumMemberName("Pendiente de pago")] PendingPayment,
    [JsonStringEnumMemberName("En preparación")] Preparing,
    [JsonStringEnumMemberName("Entregado")] Delivered,
    [JsonStringEnumMemberName("Cancelado")] Cancelled
}
