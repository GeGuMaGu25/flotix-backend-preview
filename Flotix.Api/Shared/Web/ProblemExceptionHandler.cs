using System.Text.Json;
using Flotix.Api.Shared.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Shared.Web;

/// <summary>Maps expected failures to RFC 9457 problem details without leaking internals.</summary>
public sealed class ProblemExceptionHandler(IProblemDetailsService problemDetails) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        (int Status, string Title)? problem = exception switch
        {
            DomainException => (StatusCodes.Status400BadRequest, exception.Message),
            JsonException => (StatusCodes.Status400BadRequest, "The request body is not valid."),
            DbUpdateException => (StatusCodes.Status409Conflict, "The operation conflicts with existing data."),
            _ => null
        };
        if (problem is not { } value) return false;

        context.Response.StatusCode = value.Status;
        return await problemDetails.TryWriteAsync(new()
        {
            HttpContext = context,
            ProblemDetails = { Status = value.Status, Title = value.Title }
        });
    }
}
