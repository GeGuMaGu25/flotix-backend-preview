using FlotixPlatform.API.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.API.Fuel.Application.Services;
using FlotixPlatform.API.Fuel.Domain.Model.Queries;
using FlotixPlatform.API.Fuel.Domain.Repositories;

namespace FlotixPlatform.API.Fuel.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying fuel records.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving fuel records.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="fuelRecordRepository">Repository for accessing fuel record data.</param>
public class FuelRecordQueryService(IFuelRecordRepository fuelRecordRepository)
    : IFuelRecordQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<FuelRecord>> Handle(GetAllFuelRecordsQuery query,
        CancellationToken cancellationToken = default) =>
        await fuelRecordRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<FuelRecord?> Handle(GetFuelRecordByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await fuelRecordRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<FuelRecord>> Handle(GetAllFuelRecordsByVehicleIdQuery query,
        CancellationToken cancellationToken = default) =>
        await fuelRecordRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<FuelRecord>> Handle(GetAllFuelRecordsByDriverIdQuery query,
        CancellationToken cancellationToken = default) =>
        await fuelRecordRepository.FindByDriverIdAsync(query.DriverId, cancellationToken);
}
