using System.ComponentModel.DataAnnotations;
using Flotix.Api.Fleet.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Fleet.Web;

public sealed record FuelRecordRequest(
    string VehicleId,
    string DriverId,
    [Range(0.01, 10000)] decimal Liters,
    [Range(0, 1000000)] decimal TotalCost,
    [Range(0, int.MaxValue)] int MileageAtRefuel,
    DateOnly Date) : IResourceRequest<FuelRecordRequest, FuelRecord>
{
    public FuelRecord ToEntity() => new(VehicleId, DriverId, Liters, TotalCost, MileageAtRefuel, Date);

    public void ApplyTo(FuelRecord record) => record.Update(Liters, TotalCost, MileageAtRefuel, Date);

    public static FuelRecordRequest FromEntity(FuelRecord record)
        => new(record.VehicleId, record.DriverId, record.Liters, record.TotalCost, record.MileageAtRefuel, record.Date);
}

public sealed class FuelRecordsController(IRepository<FuelRecord> records, IUnitOfWork unitOfWork)
    : CrudController<FuelRecord, FuelRecordRequest>(records, unitOfWork);
