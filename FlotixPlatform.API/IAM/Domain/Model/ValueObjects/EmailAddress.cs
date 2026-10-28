using System.Text.RegularExpressions;

namespace FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a validated email address.
/// </summary>
public sealed partial record EmailAddress
{
    private const int MaxLength = 254;

    /// <summary>
    ///     Initializes a new <see cref="EmailAddress"/> with the given raw string value.
    /// </summary>
    /// <param name="value">The raw email address. Must be non-null, non-whitespace, at most 254 characters and match the expected format.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null, whitespace, too long or malformed.</exception>
    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email address cannot be null or whitespace.", nameof(value));

        var normalizedValue = value.Trim().ToLowerInvariant();
        if (normalizedValue.Length > MaxLength)
            throw new ArgumentException($"Email address cannot exceed {MaxLength} characters.", nameof(value));

        if (!ValidFormatRegex().IsMatch(normalizedValue))
            throw new ArgumentException("Email address has an invalid format.", nameof(value));

        Value = normalizedValue;
    }

    /// <summary>
    ///     Gets the underlying primitive value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex ValidFormatRegex();
}
