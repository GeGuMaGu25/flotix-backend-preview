using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.IAM.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Identity, Profiles &amp; Security bounded context.
/// </summary>
/// <remarks>
///     The users live in the <c>iam</c> schema. Owners, drivers and mechanics share the <c>users</c> table
///     (table per hierarchy) and are told apart by the <c>role_type</c> column, as defined in the database
///     design of the project report.
/// </remarks>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Identity, Profiles &amp; Security bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyIAMConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>().ToTable(nameof(User), "iam");
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Entity<User>()
            .Property(u => u.Email)
            .HasConversion(valueObject => valueObject.Value, value => new EmailAddress(value))
            .HasMaxLength(150)
            .IsRequired();
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Entity<User>().Property(u => u.RoleType).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Entity<User>()
            .HasDiscriminator(u => u.RoleType)
            .HasValue<Owner>(Role.Owner)
            .HasValue<Driver>(Role.Driver)
            .HasValue<Mechanic>(Role.Mechanic);
        builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        builder.Entity<User>().HasIndex(u => u.RoleType);

        builder.Entity<Owner>().Property(o => o.CompanyName).HasMaxLength(100);
        builder.Entity<Driver>().Property(d => d.LicenseNumber).HasMaxLength(50);
        builder.Entity<Mechanic>().Property(m => m.WorkshopName).HasMaxLength(100);
    }
}
