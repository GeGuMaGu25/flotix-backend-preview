using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create an alert.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="AlertType">The type of the alert (e.g. Exceso de velocidad). (required)</param>
public record CreateAlertResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int VehicleId,
    [Required(ErrorMessage = "RequiredAlertType")]
    [StringLength(120, ErrorMessage = "StringLengthField")]
    string AlertType);
