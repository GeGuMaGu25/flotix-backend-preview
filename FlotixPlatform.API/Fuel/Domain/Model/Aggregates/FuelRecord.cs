using FlotixPlatform.API.Fuel.Domain.Model.Commands;

namespace FlotixPlatform.API.Fuel.Domain.Model.Aggregates;

/// <summary>
///     Represents a refuel of a vehicle registered by a driver.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateFuelRecordCommand and updated through an UpdateFuelRecordCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class FuelRecord
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected FuelRecord()
    {
    }

    /// <summary>
    ///     Creates a new FuelRecord from a creation command.
    /// </summary>
    /// <param name="command">The CreateFuelRecordCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public FuelRecord(CreateFuelRecordCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.VehicleId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.DriverId, 1);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.Liters);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Liters, 10000m);
        ArgumentOutOfRangeException.ThrowIfNegative(command.TotalCost);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.TotalCost, 1000000m);
        ArgumentOutOfRangeException.ThrowIfNegative(command.MileageAtRefuel);

        VehicleId = command.VehicleId;
        DriverId = command.DriverId;
        Liters = command.Liters;
        TotalCost = command.TotalCost;
        MileageAtRefuel = command.MileageAtRefuel;
        Date = command.Date;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this fuel record.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the vehicle.
    /// </summary>
    public int VehicleId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the driver user.
    /// </summary>
    public int DriverId { get; private set; }

    /// <summary>
    ///     Gets the refueled amount in liters.
    /// </summary>
    public decimal Liters { get; private set; }

    /// <summary>
    ///     Gets the total cost of the refuel.
    /// </summary>
    public decimal TotalCost { get; private set; }

    /// <summary>
    ///     Gets the vehicle mileage when refueling.
    /// </summary>
    public int MileageAtRefuel { get; private set; }

    /// <summary>
    ///     Gets the date of the refuel.
    /// </summary>
    public DateOnly Date { get; private set; }

    /// <summary>
    ///     Updates the fuel record information from an update command.
    /// </summary>
    /// <param name="command">The UpdateFuelRecordCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateFuelRecordCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.Liters);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Liters, 10000m);
        ArgumentOutOfRangeException.ThrowIfNegative(command.TotalCost);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.TotalCost, 1000000m);
        ArgumentOutOfRangeException.ThrowIfNegative(command.MileageAtRefuel);

        Liters = command.Liters;
        TotalCost = command.TotalCost;
        MileageAtRefuel = command.MileageAtRefuel;
        Date = command.Date;
    }
}
