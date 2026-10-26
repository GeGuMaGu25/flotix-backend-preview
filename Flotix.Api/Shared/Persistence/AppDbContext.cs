using System.Text.Json;
using Flotix.Api.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Shared.Persistence;

/// <summary>Single PostgreSQL database for the monolith; every bounded context owns its own schema.</summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        => configurationBuilder.Properties<Enum>().HaveConversion<string>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Conventions: schema = bounded context (Flotix.Api.<Context>.Domain), snake_case names, no cascading deletes.
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetSchema(SnakeCase(entity.ClrType.Namespace!.Split('.')[2]));
            entity.SetTableName(SnakeCase(entity.ClrType.Name));
            foreach (var property in entity.GetProperties()) property.SetColumnName(SnakeCase(property.Name));
            foreach (var foreignKey in entity.GetForeignKeys()) foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private static string SnakeCase(string name) => JsonNamingPolicy.SnakeCaseLower.ConvertName(name);
}
