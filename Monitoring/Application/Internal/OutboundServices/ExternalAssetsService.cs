using Hampcoders.Electrolink.API.Assets.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.OutboundServices;

public interface IExternalAssetsService
{
    Task<DeviceId?> GetDeviceBySerialNumberAsync(string serialNumber);
}

public class ExternalAssetsService(
    IAssetsContextFacade assetsFacade,
    ILogger<ExternalAssetsService> logger) : IExternalAssetsService
{
    public async Task<DeviceId?> GetDeviceBySerialNumberAsync(string serialNumber)
    {
        try
        {
            var deviceId = await assetsFacade.GetDeviceIdBySerialNumberAsync(serialNumber);
            return deviceId is not null ? DeviceId.From(deviceId) : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch device by serial number {SerialNumber}", serialNumber);
            return null;
        }
    }
}
