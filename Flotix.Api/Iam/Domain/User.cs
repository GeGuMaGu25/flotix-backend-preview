using System.Text.Json.Serialization;
using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Iam.Domain;

public sealed class User : Entity
{
    private User() { }

    public User(string name, string email, Role role, string? licenseNumber, DateOnly? licenseExpiry, string? companyName, string? workshopName)
        => Update(name, email, role, licenseNumber, licenseExpiry, companyName, workshopName);

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    [JsonIgnore]
    public string PasswordHash { get; private set; } = null!;

    public Role Role { get; private set; }

    public string? LicenseNumber { get; private set; }

    public DateOnly? LicenseExpiry { get; private set; }

    public string? CompanyName { get; private set; }

    public string? WorkshopName { get; private set; }

    public void Update(string name, string email, Role role, string? licenseNumber, DateOnly? licenseExpiry, string? companyName, string? workshopName)
    {
        if (role == Role.Driver && string.IsNullOrWhiteSpace(licenseNumber))
            throw new DomainException("A driver must have a license number.");

        Name = name.Trim();
        Email = NormalizeEmail(email);
        Role = role;
        LicenseNumber = licenseNumber;
        LicenseExpiry = licenseExpiry;
        CompanyName = companyName;
        WorkshopName = workshopName;
    }

    public void ChangePasswordHash(string passwordHash) => PasswordHash = passwordHash;

    public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
