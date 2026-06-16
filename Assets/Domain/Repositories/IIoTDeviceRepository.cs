using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Domain.Repositories;

public interface IIoTDeviceRepository : IBaseRepository<IoTDevice, IoTDeviceId>
{
    Task<IoTDevice?> FindBySerialNumberAsync(SerialNumber serialNumber);
    Task<IEnumerable<IoTDevice>> FindByStatusAsync(EDeviceStatus status);
    Task<IEnumerable<IoTDevice>> FindByPropertyIdAsync(PropertyId propertyId);
    Task<int> CountActiveByOwnerIdAsync(string ownerId);
    Task<IoTDevice?> FindFirstInStockAsync();
}
