using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Commerce.Domain.Model.Aggregates;

/// <summary>
///     Represents an IoT product offered in the store catalog.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateIotCatalogItemCommand and updated through an UpdateIotCatalogItemCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class IotCatalogItem
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected IotCatalogItem()
    {
        Sku = null!;
        Name = string.Empty;
        IconName = string.Empty;
    }

    /// <summary>
    ///     Creates a new IotCatalogItem from a creation command.
    /// </summary>
    /// <param name="command">The CreateIotCatalogItemCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public IotCatalogItem(CreateIotCatalogItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Sku);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Name.Length, 120);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.UnitPrice);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.UnitPrice, 100000m);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.IconName);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.IconName.Length, 40);

        Sku = command.Sku;
        Name = command.Name;
        UnitPrice = command.UnitPrice;
        IconName = command.IconName;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this IoT catalog item.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the stock keeping unit of the IoT product.
    /// </summary>
    public Sku Sku { get; private set; }

    /// <summary>
    ///     Gets the name of the IoT product.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets the unit price of the IoT product.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    ///     Gets the icon used to display the product.
    /// </summary>
    public string IconName { get; private set; }

    /// <summary>
    ///     Updates the IoT catalog item information from an update command.
    /// </summary>
    /// <param name="command">The UpdateIotCatalogItemCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateIotCatalogItemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Name.Length, 120);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.UnitPrice);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.UnitPrice, 100000m);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.IconName);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.IconName.Length, 40);

        Name = command.Name;
        UnitPrice = command.UnitPrice;
        IconName = command.IconName;
    }
}
