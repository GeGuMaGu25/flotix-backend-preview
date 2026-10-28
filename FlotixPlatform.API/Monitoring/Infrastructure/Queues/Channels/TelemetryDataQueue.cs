using System.Threading.Channels;
using FlotixPlatform.API.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.API.Monitoring.Domain.Model.Commands;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Queues.Channels;

/// <summary>
///     In-memory telemetry ingestion queue based on a bounded <see cref="Channel{T}"/>.
/// </summary>
/// <remarks>
///     When the queue is full, reception waits until the processor frees space (back-pressure) instead of
///     dropping packets.
/// </remarks>
public class TelemetryDataQueue : ITelemetryDataQueue
{
    private const int Capacity = 10_000;

    private readonly Channel<ReceiveTelemetryDataCommand> _channel =
        Channel.CreateBounded<ReceiveTelemetryDataCommand>(new BoundedChannelOptions(Capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true
        });

    /// <inheritdoc />
    public ValueTask EnqueueAsync(ReceiveTelemetryDataCommand command, CancellationToken cancellationToken = default) =>
        _channel.Writer.WriteAsync(command, cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<ReceiveTelemetryDataCommand> DequeueAllAsync(CancellationToken cancellationToken) =>
        _channel.Reader.ReadAllAsync(cancellationToken);
}
