using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Store.Domain;

public sealed class IotOrder : Entity
{
    private IotOrder() { }

    /// <summary>Places an order priced from the catalog, never from the client.</summary>
    public IotOrder(string ownerId, IotCatalogItem item, int deviceQuantity)
    {
        if (deviceQuantity <= 0) throw new DomainException("An order needs at least one device.");

        OwnerId = ownerId;
        Sku = item.Sku;
        DeviceQuantity = deviceQuantity;
        TotalAmount = item.UnitPrice * deviceQuantity;
        Status = OrderStatus.PendingPayment;
        OrderDate = DateOnly.FromDateTime(CreatedAt.UtcDateTime);
    }

    public string OwnerId { get; private set; } = null!;

    public string Sku { get; private set; } = null!;

    public int DeviceQuantity { get; private set; }

    public decimal TotalAmount { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateOnly OrderDate { get; private set; }

    public void ChangeStatus(OrderStatus status)
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled && status != Status)
            throw new DomainException("A delivered or cancelled order cannot change its status.");

        Status = status;
    }
}
