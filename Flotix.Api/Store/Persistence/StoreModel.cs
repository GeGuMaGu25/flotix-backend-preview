using Flotix.Api.Iam.Domain;
using Flotix.Api.Store.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Store.Persistence;

public static class StoreModel
{
    private static readonly DateTimeOffset SeededAt = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public static ModelBuilder ApplyStoreModel(this ModelBuilder model)
    {
        model.Entity<IotCatalogItem>(item =>
        {
            item.HasIndex(i => i.Sku).IsUnique();
            item.HasData(
                CatalogItem("gps", "Rastreador GPS Flotix", 149.9m, "pin"),
                CatalogItem("odometer", "Odómetro digital Flotix", 89.9m, "signal"),
                CatalogItem("fuel-sensor", "Sensor de combustible Flotix", 119.9m, "fuel"),
                CatalogItem("obd-ii", "Lector de diagnóstico OBD-II Flotix", 99.9m, "chip"),
                CatalogItem("dashcam", "Cámara de cabina Flotix", 189.9m, "camera"),
                CatalogItem("tpms", "Sensor de presión de llantas Flotix", 129.9m, "gear"),
                CatalogItem("immobilizer", "Inmovilizador remoto Flotix", 169.9m, "shield"),
                CatalogItem("panic-button", "Botón de pánico Flotix", 59.9m, "bell"),
                CatalogItem("temp-sensor", "Sensor de temperatura de carga Flotix", 109.9m, "chart"),
                CatalogItem("driver-id", "Lector de identificación de conductor Flotix", 79.9m, "users"));
        });

        model.Entity<IotOrder>(order =>
        {
            order.HasOne<User>().WithMany().HasForeignKey(o => o.OwnerId);
            order.HasOne<IotCatalogItem>().WithMany().HasForeignKey(o => o.Sku);
        });

        return model;
    }

    private static object CatalogItem(string sku, string name, decimal unitPrice, string iconName)
        => new { Id = sku, CreatedAt = SeededAt, Sku = sku, Name = name, UnitPrice = unitPrice, IconName = iconName };
}
