using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Store.Domain;

/// <summary>A device sold in the IoT store; its SKU is its identity.</summary>
public sealed class IotCatalogItem : Entity
{
    private IotCatalogItem() { }

    public IotCatalogItem(string sku, string name, decimal unitPrice, string iconName) : base(sku)
    {
        Sku = sku;
        Update(name, unitPrice, iconName);
    }

    public string Sku { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public decimal UnitPrice { get; private set; }

    public string IconName { get; private set; } = null!;

    public void Update(string name, decimal unitPrice, string iconName)
    {
        Name = name;
        UnitPrice = unitPrice;
        IconName = iconName;
    }
}
