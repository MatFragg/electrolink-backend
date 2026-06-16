using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class IoTDeviceResourceFromEntityAssembler
{
    public static IoTDeviceResource ToResourceFromEntity(IoTDevice entity)
        => new(
            entity.Id.Value,
            entity.SerialNumber.Value,
            entity.FirmwareVersion,
            entity.Status,
            entity.AssignedPropertyId?.Value,
            entity.ConnectionStatus,
            entity.LastReadingAt,
            entity.InstalledAt);
}
