using System.ComponentModel.DataAnnotations;
using Flotix.Api.Store.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Store.Web;

public sealed record IotCatalogItemRequest(
    [RegularExpression("^[a-z0-9-]{1,40}$")] string Sku,
    [StringLength(120, MinimumLength = 1)] string Name,
    [Range(0.01, 100000)] decimal UnitPrice,
    [StringLength(40, MinimumLength = 1)] string IconName) : IResourceRequest<IotCatalogItemRequest, IotCatalogItem>
{
    public IotCatalogItem ToEntity() => new(Sku, Name, UnitPrice, IconName);

    public void ApplyTo(IotCatalogItem item) => item.Update(Name, UnitPrice, IconName);

    public static IotCatalogItemRequest FromEntity(IotCatalogItem item) => new(item.Sku, item.Name, item.UnitPrice, item.IconName);
}

public sealed class IotCatalogController(IRepository<IotCatalogItem> catalog, IUnitOfWork unitOfWork)
    : CrudController<IotCatalogItem, IotCatalogItemRequest>(catalog, unitOfWork);
