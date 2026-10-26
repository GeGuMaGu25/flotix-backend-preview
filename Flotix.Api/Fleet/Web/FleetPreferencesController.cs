using System.ComponentModel.DataAnnotations;
using Flotix.Api.Fleet.Domain;
using Flotix.Api.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Flotix.Api.Fleet.Web;

public sealed record FleetPreferencesRequest(
    [Range(1, 300)] int SpeedLimitKmh,
    [Range(1, 1000000)] int MaintenanceIntervalKm,
    [Range(1, 100)] int FuelAnomalyDropPct);

/// <summary>Singular resource, as in the fake API: <c>GET</c>/<c>PUT /api/v1/fleet-preferences</c>.</summary>
[ApiController]
[Route("api/v1/[controller]")]
public sealed class FleetPreferencesController(IRepository<FleetPreferences> preferences, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<FleetPreferences>> Get(CancellationToken cancellationToken)
        => await preferences.FindAsync(FleetPreferences.DefaultId, cancellationToken) is { } current ? current : NotFound();

    [HttpPut]
    public async Task<ActionResult<FleetPreferences>> Update(FleetPreferencesRequest request, CancellationToken cancellationToken)
    {
        if (await preferences.FindAsync(FleetPreferences.DefaultId, cancellationToken) is not { } current) return NotFound();
        current.Update(request.SpeedLimitKmh, request.MaintenanceIntervalKm, request.FuelAnomalyDropPct);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return current;
    }
}
