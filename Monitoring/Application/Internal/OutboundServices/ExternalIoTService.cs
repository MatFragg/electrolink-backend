using Hampcoders.Electrolink.API.Processing.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.OutboundServices;

public interface IExternalIoTService
{
    Task IssueRelayCommandAsync(string executionId, string deviceId, string command, string issuedBy);
}

public class ExternalIoTService(
    IProcessingContextFacade processingFacade,
    ILogger<ExternalIoTService> logger) : IExternalIoTService
{
    public async Task IssueRelayCommandAsync(string executionId, string deviceId, string command, string issuedBy)
    {
        try
        {
            await processingFacade.RequestRelayToggleAsync(
                executionId, deviceId, issuedBy, command, "Remote circuit toggle", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to issue relay command {Command} for device {DeviceId}.", command, deviceId);
        }
    }
}
