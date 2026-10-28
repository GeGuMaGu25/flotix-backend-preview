using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace FlotixPlatform.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for fleet preferences persistence.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class FleetPreferencesRepository(AppDbContext context)
    : BaseRepository<FleetPreferences>(context), IFleetPreferencesRepository;
