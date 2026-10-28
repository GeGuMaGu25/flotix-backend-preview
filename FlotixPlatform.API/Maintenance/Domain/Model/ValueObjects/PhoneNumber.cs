using System.Text.RegularExpressions;

namespace FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a validated phone number.
/// </summary>
public sealed partial record PhoneNumber
{
    private const int MaxLength = 30;

    /// <summary>
    ///     Initializes a new <see cref="PhoneNumber"/> with the given raw string value.
    /// </summary>
    /// <param name="value">The raw phone number. Must be non-null, non-whitespace, at most 30 characters and match the expected format.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null, whitespace, too long or malformed.</exception>
    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number cannot be null or whitespace.", nameof(value));

        var normalizedValue = value.Trim();
        if (normalizedValue.Length > MaxLength)
            throw new ArgumentException($"Phone number cannot exceed {MaxLength} characters.", nameof(value));

        if (!ValidFormatRegex().IsMatch(normalizedValue))
            throw new ArgumentException("Phone number has an invalid format.", nameof(value));

        Value = normalizedValue;
    }

    /// <summary>
    ///     Gets the underlying primitive value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;

    [GeneratedRegex(@"^\+?[0-9 ()-]{6,30}$")]
    private static partial Regex ValidFormatRegex();
}
