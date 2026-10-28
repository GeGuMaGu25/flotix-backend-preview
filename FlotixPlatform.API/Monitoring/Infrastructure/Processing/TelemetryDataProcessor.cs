using FlotixPlatform.API.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.API.Monitoring.Application.Services;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Processing;

/// <summary>
///     IoT Telemetry Processor: processes in the background the telemetry packets received from the IoT devices.
/// </summary>
/// <remarks>
///     Each packet is processed in its own dependency injection scope, so a failing packet never stops the
///     processing of the following ones. Discarded packets are logged by the command service.
/// </remarks>
/// <param name="telemetryDataQueue">The telemetry ingestion queue.</param>
/// <param name="serviceScopeFactory">Factory used to create a scope per packet.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class TelemetryDataProcessor(
    ITelemetryDataQueue telemetryDataQueue,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<TelemetryDataProcessor> logger)
    : BackgroundService
{
    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var command in telemetryDataQueue.DequeueAllAsync(stoppingToken))
        {
            try
            {
                await using var scope = serviceScopeFactory.CreateAsyncScope();
                var telemetryDataCommandService = scope.ServiceProvider.GetRequiredService<ITelemetryDataCommandService>();
                await telemetryDataCommandService.Process(command, stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Unexpected error in the IoT Telemetry Processor for device {DeviceId}", command.DeviceId);
            }
        }
    }
}
