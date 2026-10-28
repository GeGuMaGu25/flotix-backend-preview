using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Analytics.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a report.
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="ReportType">The type of the report (e.g. Reporte de incidencias). (required)</param>
/// <param name="FilterParams">The filters applied to the report.</param>
public record CreateReportResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int OwnerId,
    [Required(ErrorMessage = "RequiredReportType")]
    [StringLength(120, ErrorMessage = "StringLengthField")]
    string ReportType,
    [MaxLength(10, ErrorMessage = "MaxLengthField")]
    Dictionary<string, string>? FilterParams);
