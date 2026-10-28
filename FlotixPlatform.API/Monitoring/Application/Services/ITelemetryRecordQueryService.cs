using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Domain.Model.Queries;

namespace FlotixPlatform.API.Monitoring.Application.Services;

/// <summary>
///     Query service interface for telemetry record operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying telemetry records by various filters.
/// </remarks>
public interface ITelemetryRecordQueryService
{
	/// <summary>
	///     Handle the GetAllTelemetryRecordsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllTelemetryRecordsQuery. It returns all the telemetry records.
	/// </remarks>
	/// <param name="query">The GetAllTelemetryRecordsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the TelemetryRecord objects</returns>
	Task<IEnumerable<TelemetryRecord>> Handle(GetAllTelemetryRecordsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetTelemetryRecordByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetTelemetryRecordByIdQuery. It returns the telemetry record for the given Id.
	/// </remarks>
	/// <param name="query">The GetTelemetryRecordByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The TelemetryRecord object if found, or null otherwise</returns>
	Task<TelemetryRecord?> Handle(GetTelemetryRecordByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllTelemetryRecordsByDeviceIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllTelemetryRecordsByDeviceIdQuery. It returns all the telemetry records for the given device ID.
	/// </remarks>
	/// <param name="query">The GetAllTelemetryRecordsByDeviceIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the TelemetryRecord objects</returns>
	Task<IEnumerable<TelemetryRecord>> Handle(GetAllTelemetryRecordsByDeviceIdQuery query, CancellationToken cancellationToken = default);
}
