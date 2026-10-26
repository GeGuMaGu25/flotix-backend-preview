using System.ComponentModel.DataAnnotations;
using Flotix.Api.Iam.Domain;
using Flotix.Api.Iam.Security;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;
using Flotix.Api.Store.Domain;

namespace Flotix.Api.Store.Web;

/// <remarks>The total and order date are computed by the domain; only the status of a placed order can change.</remarks>
public sealed record IotOrderRequest(
    string OwnerId,
    string Sku,
    [Range(1, 1000)] int DeviceQuantity,
    OrderStatus Status = OrderStatus.PendingPayment) : IResourceRequest<IotOrderRequest, IotOrder>
{
    public IotOrder ToEntity() => throw new NotSupportedException("An order must be priced against the catalog.");

    public void ApplyTo(IotOrder order) => order.ChangeStatus(Status);

    public static IotOrderRequest FromEntity(IotOrder order) => new(order.OwnerId, order.Sku, order.DeviceQuantity, order.Status);
}

[AllowRoles(Role.Owner)]
public sealed class IotOrdersController(IRepository<IotOrder> orders, IRepository<IotCatalogItem> catalog, IUnitOfWork unitOfWork)
    : CrudController<IotOrder, IotOrderRequest>(orders, unitOfWork)
{
    protected override async Task<IotOrder> CreateEntityAsync(IotOrderRequest request, CancellationToken cancellationToken)
        => await catalog.FindAsync(request.Sku, cancellationToken) is { } item
            ? new IotOrder(request.OwnerId, item, request.DeviceQuantity)
            : throw new DomainException($"The SKU '{request.Sku}' is not in the catalog.");
}
