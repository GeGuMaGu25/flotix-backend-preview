using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Commerce.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Digital Experience / IoT Commerce bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Digital Experience / IoT Commerce bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyCommerceConfiguration(this ModelBuilder builder)
    {
        builder.Entity<IotOrder>().HasKey(i => i.Id);
        builder.Entity<IotOrder>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<IotOrder>().Property(i => i.OwnerId).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.DeviceQuantity).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.TotalAmount).HasPrecision(12, 2).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.OrderDate).IsRequired();
        builder.Entity<IotOrder>().HasIndex(i => i.OwnerId);
    }
}
