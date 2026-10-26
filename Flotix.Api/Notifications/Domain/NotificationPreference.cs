using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Notifications.Domain;

/// <summary>Which kinds of notification a user wants to receive.</summary>
public sealed class NotificationPreference : Entity
{
    private NotificationPreference() { }

    public NotificationPreference(string userId, bool maintenance, bool speed, bool incidents)
    {
        UserId = userId;
        Update(maintenance, speed, incidents);
    }

    public string UserId { get; private set; } = null!;

    public bool Maintenance { get; private set; }

    public bool Speed { get; private set; }

    public bool Incidents { get; private set; }

    public void Update(bool maintenance, bool speed, bool incidents)
    {
        Maintenance = maintenance;
        Speed = speed;
        Incidents = incidents;
    }
}
