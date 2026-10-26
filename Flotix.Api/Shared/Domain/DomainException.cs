namespace Flotix.Api.Shared.Domain;

/// <summary>Raised when an operation would break a business rule of an aggregate.</summary>
public sealed class DomainException(string message) : Exception(message);
