using FlotixPlatform.API.IAM.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Fuel.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Maintenance.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Commerce.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Application database context shared by every bounded context.
/// </summary>
/// <remarks>
///     Each bounded context contributes its entity configuration through its own ModelBuilder extension.
///     Table, column, key and index names are converted to snake_case by convention.
/// </remarks>
/// <param name="options">The options of the database context.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyIAMConfiguration();
        builder.ApplyFleetConfiguration();
        builder.ApplyFuelConfiguration();
        builder.ApplyMaintenanceConfiguration();
        builder.ApplyIncidentsConfiguration();
        builder.ApplyMonitoringConfiguration();
        builder.ApplyNotificationsConfiguration();
        builder.ApplyCommerceConfiguration();

        builder.UseSnakeCaseNamingConvention();
    }
}
