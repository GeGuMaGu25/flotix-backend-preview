using System.ComponentModel.DataAnnotations;
using Flotix.Api.Monitoring.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Monitoring.Web;

public sealed record IotDeviceRequest(
    string VehicleId,
    [StringLength(40, MinimumLength = 1)] string SerialNumber,
    ConnectionStatus ConnectionStatus = ConnectionStatus.Disconnected) : IResourceRequest<IotDeviceRequest, IotDevice>
{
    public IotDevice ToEntity() => new(VehicleId, SerialNumber, ConnectionStatus);

    public void ApplyTo(IotDevice device) => device.Update(VehicleId, ConnectionStatus);

    public static IotDeviceRequest FromEntity(IotDevice device) => new(device.VehicleId, device.SerialNumber, device.ConnectionStatus);
}

public sealed class IotDevicesController(IRepository<IotDevice> devices, IUnitOfWork unitOfWork)
    : CrudController<IotDevice, IotDeviceRequest>(devices, unitOfWork);
