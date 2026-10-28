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
        builder.Entity<IotCatalogItem>().HasKey(i => i.Id);
        builder.Entity<IotCatalogItem>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<IotCatalogItem>()
            .Property(i => i.Sku)
            .HasConversion(valueObject => valueObject.Value, value => new Sku(value))
            .HasMaxLength(40)
            .IsRequired();
        builder.Entity<IotCatalogItem>().Property(i => i.Name).IsRequired().HasMaxLength(120);
        builder.Entity<IotCatalogItem>().Property(i => i.UnitPrice).HasPrecision(12, 2).IsRequired();
        builder.Entity<IotCatalogItem>().Property(i => i.IconName).IsRequired().HasMaxLength(40);
        builder.Entity<IotCatalogItem>().HasIndex(i => i.Sku).IsUnique();
        builder.Entity<IotCatalogItem>().HasData(
            new { Id = 1, Sku = new Sku("gps"), Name = "Rastreador GPS Flotix", UnitPrice = 149.9m, IconName = "pin" },
            new { Id = 2, Sku = new Sku("odometer"), Name = "Odómetro digital Flotix", UnitPrice = 89.9m, IconName = "signal" },
            new { Id = 3, Sku = new Sku("fuel-sensor"), Name = "Sensor de combustible Flotix", UnitPrice = 119.9m, IconName = "fuel" },
            new { Id = 4, Sku = new Sku("obd-ii"), Name = "Lector de diagnóstico OBD-II Flotix", UnitPrice = 99.9m, IconName = "chip" },
            new { Id = 5, Sku = new Sku("dashcam"), Name = "Cámara de cabina Flotix", UnitPrice = 189.9m, IconName = "camera" },
            new { Id = 6, Sku = new Sku("tpms"), Name = "Sensor de presión de llantas Flotix", UnitPrice = 129.9m, IconName = "gear" },
            new { Id = 7, Sku = new Sku("immobilizer"), Name = "Inmovilizador remoto Flotix", UnitPrice = 169.9m, IconName = "shield" },
            new { Id = 8, Sku = new Sku("panic-button"), Name = "Botón de pánico Flotix", UnitPrice = 59.9m, IconName = "bell" },
            new { Id = 9, Sku = new Sku("temp-sensor"), Name = "Sensor de temperatura de carga Flotix", UnitPrice = 109.9m, IconName = "chart" },
            new { Id = 10, Sku = new Sku("driver-id"), Name = "Lector de identificación de conductor Flotix", UnitPrice = 79.9m, IconName = "users" });

        builder.Entity<IotOrder>().HasKey(i => i.Id);
        builder.Entity<IotOrder>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<IotOrder>().Property(i => i.OwnerId).IsRequired();
        builder.Entity<IotOrder>()
            .Property(i => i.Sku)
            .HasConversion(valueObject => valueObject.Value, value => new Sku(value))
            .HasMaxLength(40)
            .IsRequired();
        builder.Entity<IotOrder>().Property(i => i.DeviceQuantity).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.TotalAmount).HasPrecision(12, 2).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Entity<IotOrder>().Property(i => i.OrderDate).IsRequired();
        builder.Entity<IotOrder>().HasIndex(i => i.OwnerId);
    }
}
