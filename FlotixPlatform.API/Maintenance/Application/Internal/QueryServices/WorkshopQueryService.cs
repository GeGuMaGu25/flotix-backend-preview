using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Application.Services;
using FlotixPlatform.API.Maintenance.Domain.Model.Queries;
using FlotixPlatform.API.Maintenance.Domain.Repositories;

namespace FlotixPlatform.API.Maintenance.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying workshops.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving workshops.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="workshopRepository">Repository for accessing workshop data.</param>
public class WorkshopQueryService(IWorkshopRepository workshopRepository)
    : IWorkshopQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Workshop>> Handle(GetAllWorkshopsQuery query,
        CancellationToken cancellationToken = default) =>
        await workshopRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Workshop?> Handle(GetWorkshopByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await workshopRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Workshop>> Handle(GetAllWorkshopsByMechanicIdQuery query,
        CancellationToken cancellationToken = default) =>
        await workshopRepository.FindByMechanicIdAsync(query.MechanicId, cancellationToken);
}
