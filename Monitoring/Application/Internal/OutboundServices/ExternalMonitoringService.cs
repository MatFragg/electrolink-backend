using Hampcoders.Electrolink.API.Processing.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.OutboundServices;

public interface IExternalMonitoringService
{
    Task PublishCircuitToggleRequestAsync(string executionId, string deviceId, string targetState);
}

public class ExternalMonitoringService(
    IProcessingContextFacade processingFacade,
    ILogger<ExternalMonitoringService> logger) : IExternalMonitoringService
{
    public async Task PublishCircuitToggleRequestAsync(string executionId, string deviceId, string targetState)
    {
        try
        {
            await processingFacade.RequestRelayToggleAsync(
                executionId, deviceId, "SYSTEM", targetState, "Circuit toggle from monitoring", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to publish circuit toggle request for service {ExecutionId} device {DeviceId}.",
                executionId, deviceId);
        }
    }
}
