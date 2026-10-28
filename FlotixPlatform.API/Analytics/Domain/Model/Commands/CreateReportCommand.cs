namespace FlotixPlatform.API.Analytics.Domain.Model.Commands;

/// <summary>
///     Command to create a report
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="ReportType">The type of the report (e.g. Reporte de incidencias).</param>
/// <param name="FilterParams">The filters applied to the report.</param>
public record CreateReportCommand(int OwnerId, string ReportType, Dictionary<string, string> FilterParams);
