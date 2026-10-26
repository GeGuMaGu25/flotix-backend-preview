using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Notifications.Domain;

public sealed class Notification : Entity
{
    private Notification() { }

    public Notification(string userId, string? alertId, string message)
    {
        UserId = userId;
        AlertId = alertId;
        Message = message;
        SentAt = CreatedAt;
    }

    public string UserId { get; private set; } = null!;

    public string? AlertId { get; private set; }

    public string Message { get; private set; } = null!;

    public bool IsRead { get; private set; }

    public DateTimeOffset SentAt { get; private set; }

    public void MarkAs(bool isRead) => IsRead = isRead;
}
