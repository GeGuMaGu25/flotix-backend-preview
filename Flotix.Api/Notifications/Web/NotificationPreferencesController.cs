using Flotix.Api.Notifications.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Notifications.Web;

public sealed record NotificationPreferenceRequest(string UserId, bool Maintenance = true, bool Speed = true, bool Incidents = true)
    : IResourceRequest<NotificationPreferenceRequest, NotificationPreference>
{
    public NotificationPreference ToEntity() => new(UserId, Maintenance, Speed, Incidents);

    public void ApplyTo(NotificationPreference preference) => preference.Update(Maintenance, Speed, Incidents);

    public static NotificationPreferenceRequest FromEntity(NotificationPreference preference)
        => new(preference.UserId, preference.Maintenance, preference.Speed, preference.Incidents);
}

public sealed class NotificationPreferencesController(IRepository<NotificationPreference> preferences, IUnitOfWork unitOfWork)
    : CrudController<NotificationPreference, NotificationPreferenceRequest>(preferences, unitOfWork);
