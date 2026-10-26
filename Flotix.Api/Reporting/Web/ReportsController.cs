using System.ComponentModel.DataAnnotations;
using Flotix.Api.Iam.Domain;
using Flotix.Api.Iam.Security;
using Flotix.Api.Reporting.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Reporting.Web;

public sealed record ReportRequest(
    string OwnerId,
    [StringLength(120, MinimumLength = 1)] string ReportType,
    [MaxLength(10)] Dictionary<string, string>? FilterParams = null) : IResourceRequest<ReportRequest, Report>
{
    public Report ToEntity() => new(OwnerId, ReportType, FilterParams ?? []);

    public void ApplyTo(Report report) => report.Update(ReportType, FilterParams ?? []);

    public static ReportRequest FromEntity(Report report) => new(report.OwnerId, report.ReportType, report.FilterParams);
}

[AllowRoles(Role.Owner)]
public sealed class ReportsController(IRepository<Report> reports, IUnitOfWork unitOfWork)
    : CrudController<Report, ReportRequest>(reports, unitOfWork);
