using System.Text.RegularExpressions;

namespace FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a validated SKU.
/// </summary>
public sealed partial record Sku
{
    private const int MaxLength = 40;

    /// <summary>
    ///     Initializes a new <see cref="Sku"/> with the given raw string value.
    /// </summary>
    /// <param name="value">The raw SKU. Must be non-null, non-whitespace, at most 40 characters and match the expected format.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null, whitespace, too long or malformed.</exception>
    public Sku(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SKU cannot be null or whitespace.", nameof(value));

        var normalizedValue = value.Trim().ToLowerInvariant();
        if (normalizedValue.Length > MaxLength)
            throw new ArgumentException($"SKU cannot exceed {MaxLength} characters.", nameof(value));

        if (!ValidFormatRegex().IsMatch(normalizedValue))
            throw new ArgumentException("SKU has an invalid format.", nameof(value));

        Value = normalizedValue;
    }

    /// <summary>
    ///     Gets the underlying primitive value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;

    [GeneratedRegex(@"^[a-z0-9-]+$")]
    private static partial Regex ValidFormatRegex();
}
