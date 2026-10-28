namespace FlotixPlatform.API.Analytics.Domain.Model.Commands;

/// <summary>
///     Command to create a report
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="ReportType">The type of the report (e.g. Reporte de incidencias).</param>
/// <param name="FilterParams">The filters applied to the report (e.g. date range).</param>
public record CreateReportCommand(Guid OwnerId, string ReportType, string FilterParams);
