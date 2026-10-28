using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a notification.
/// </summary>
/// <param name="UserId">The identifier of the recipient user.</param>
/// <param name="AlertId">The identifier of the related alert, if any.</param>
/// <param name="Message">The message of the notification. (required)</param>
public record CreateNotificationResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int UserId,
    int? AlertId,
    [Required(ErrorMessage = "RequiredMessage")]
    [StringLength(500, ErrorMessage = "StringLengthField")]
    string Message);
