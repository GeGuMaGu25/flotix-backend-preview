using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Monitoring.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Monitoring.Application.Services;

/// <summary>
///     Command service interface for telemetry record operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change telemetry records.
/// </remarks>
public interface ITelemetryDataCommandService
{
    /// <summary>
    ///     Handle the create telemetry record command.
    /// </summary>
    /// <param name="command">CreateTelemetryDataCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created TelemetryData on success, or the error reason on failure.
    /// </returns>
    Task<Result<TelemetryData, CreateTelemetryDataError>> Handle(CreateTelemetryDataCommand command, CancellationToken cancellationToken = default);
}
