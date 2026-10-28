using FlotixPlatform.API.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.API.Analytics.Domain.Model.Commands;
using FlotixPlatform.API.Analytics.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Analytics.Application.Services;

/// <summary>
///     Command service interface for report operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change reports.
/// </remarks>
public interface IReportCommandService
{
    /// <summary>
    ///     Handle the create report command.
    /// </summary>
    /// <param name="command">CreateReportCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created Report on success, or the error reason on failure.
    /// </returns>
    Task<Result<Report, CreateReportError>> Handle(CreateReportCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete report command.
    /// </summary>
    /// <param name="command">DeleteReportCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted Report on success, or the error reason on failure.
    /// </returns>
    Task<Result<Report, DeleteReportError>> Handle(DeleteReportCommand command, CancellationToken cancellationToken = default);
}
