using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Domain.Model.Queries;

namespace FlotixPlatform.API.Maintenance.Application.Services;

/// <summary>
///     Query service interface for workshop operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying workshops by various filters.
/// </remarks>
public interface IWorkshopQueryService
{
	/// <summary>
	///     Handle the GetAllWorkshopsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllWorkshopsQuery. It returns all the workshops.
	/// </remarks>
	/// <param name="query">The GetAllWorkshopsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Workshop objects</returns>
	Task<IEnumerable<Workshop>> Handle(GetAllWorkshopsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetWorkshopByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetWorkshopByIdQuery. It returns the workshop for the given Id.
	/// </remarks>
	/// <param name="query">The GetWorkshopByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The Workshop object if found, or null otherwise</returns>
	Task<Workshop?> Handle(GetWorkshopByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllWorkshopsByMechanicIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllWorkshopsByMechanicIdQuery. It returns all the workshops for the given mechanic ID.
	/// </remarks>
	/// <param name="query">The GetAllWorkshopsByMechanicIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the Workshop objects</returns>
	Task<IEnumerable<Workshop>> Handle(GetAllWorkshopsByMechanicIdQuery query, CancellationToken cancellationToken = default);
}
