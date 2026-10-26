using System.Text.Json.Serialization;

namespace Flotix.Api.Monitoring.Domain;

public enum ConnectionStatus
{
    [JsonStringEnumMemberName("Conectado")] Connected,
    [JsonStringEnumMemberName("Desconectado")] Disconnected
}
