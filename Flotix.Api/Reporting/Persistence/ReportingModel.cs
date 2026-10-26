using Flotix.Api.Iam.Domain;
using Flotix.Api.Reporting.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Reporting.Persistence;

public static class ReportingModel
{
    public static ModelBuilder ApplyReportingModel(this ModelBuilder model)
    {
        model.Entity<Report>(report =>
        {
            report.Property(r => r.FilterParams).HasColumnType("jsonb");
            report.HasOne<User>().WithMany().HasForeignKey(r => r.OwnerId);
        });

        return model;
    }
}
