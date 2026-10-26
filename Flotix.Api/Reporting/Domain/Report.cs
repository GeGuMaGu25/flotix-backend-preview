using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Reporting.Domain;

public sealed class Report : Entity
{
    private Report() { }

    public Report(string ownerId, string reportType, Dictionary<string, string> filterParams)
    {
        OwnerId = ownerId;
        GeneratedAt = CreatedAt;
        Update(reportType, filterParams);
    }

    public string OwnerId { get; private set; } = null!;

    public string ReportType { get; private set; } = null!;

    public DateTimeOffset GeneratedAt { get; private set; }

    public Dictionary<string, string> FilterParams { get; private set; } = [];

    public void Update(string reportType, Dictionary<string, string> filterParams)
    {
        ReportType = reportType;
        FilterParams = filterParams;
    }
}
