using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;

public class IoTDeviceQueryService : IIoTDeviceQueryService
{
    private readonly IIoTDeviceRepository _deviceRepository;

    public IoTDeviceQueryService(IIoTDeviceRepository deviceRepository)
        => _deviceRepository = deviceRepository;

    public Task<IoTDevice?> Handle(GetIoTDeviceByIdQuery query)
        => _deviceRepository.FindByIdAsync(IoTDeviceId.From(query.DeviceId));

    public Task<IoTDevice?> Handle(GetIoTDeviceBySerialNumberQuery query)
        => _deviceRepository.FindBySerialNumberAsync(SerialNumber.From(query.SerialNumber));

    public async Task<IEnumerable<IoTDevice>> Handle(GetAllIoTDevicesQuery query)
        => await _deviceRepository.ListAsync();

    public Task<IEnumerable<IoTDevice>> Handle(GetIoTDevicesByPropertyIdQuery query)
        => _deviceRepository.FindByPropertyIdAsync(PropertyId.From(query.PropertyId));

    public Task<IEnumerable<IoTDevice>> Handle(GetIoTDevicesByStatusQuery query)
        => _deviceRepository.FindByStatusAsync(query.Status);

    public Task<int> Handle(GetActiveDeviceCountByOwnerIdQuery query)
        => _deviceRepository.CountActiveByOwnerIdAsync(query.OwnerId);
}
