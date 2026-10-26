using System.Text.Json;

namespace Flotix.Api.Shared.Web;

/// <summary>Turns the <c>[controller]</c>/<c>[action]</c> route tokens into kebab-case (FuelRecords → fuel-records).</summary>
public sealed class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
        => value?.ToString() is { } name ? JsonNamingPolicy.KebabCaseLower.ConvertName(name) : null;
}
