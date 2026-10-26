using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Monitoring.Domain;

public sealed class Alert : Entity
{
    private Alert() { }

    public Alert(string vehicleId, string alertType)
    {
        VehicleId = vehicleId;
        AlertType = alertType;
        ActivatedAt = CreatedAt;
    }

    public string VehicleId { get; private set; } = null!;

    public string AlertType { get; private set; } = null!;

    public DateTimeOffset ActivatedAt { get; private set; }
}
