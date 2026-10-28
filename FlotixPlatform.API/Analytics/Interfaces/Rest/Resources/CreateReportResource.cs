using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Analytics.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a report.
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="ReportType">The type of the report (e.g. Reporte de incidencias). (required)</param>
/// <param name="FilterParams">The filters applied to the report (e.g. date range). (required)</param>
public record CreateReportResource(
    Guid OwnerId,
    [Required(ErrorMessage = "RequiredReportType")]
    [StringLength(50, ErrorMessage = "StringLengthField")]
    string ReportType,
    [Required(ErrorMessage = "RequiredFilterParams")]
    [StringLength(2000, ErrorMessage = "StringLengthField")]
    string FilterParams);
