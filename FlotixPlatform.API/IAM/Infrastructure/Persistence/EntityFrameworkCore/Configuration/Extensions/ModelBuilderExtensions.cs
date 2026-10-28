using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.IAM.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Identity, Profiles &amp; Security bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Identity, Profiles &amp; Security bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyIAMConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>()
            .Property(u => u.Email)
            .HasConversion(valueObject => valueObject.Value, value => new EmailAddress(value))
            .HasMaxLength(254)
            .IsRequired();
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Entity<User>().Property(u => u.Role).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Entity<User>().Property(u => u.FullName).IsRequired().HasMaxLength(120);
        builder.Entity<User>().Property(u => u.LicenseNumber).HasMaxLength(20);
        builder.Entity<User>().Property(u => u.CompanyName).HasMaxLength(120);
        builder.Entity<User>().Property(u => u.WorkshopName).HasMaxLength(120);
        builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        builder.Entity<User>().HasIndex(u => u.Role);
    }
}
