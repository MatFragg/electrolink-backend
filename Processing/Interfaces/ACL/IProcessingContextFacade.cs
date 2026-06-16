using Hampcoders.Electrolink.API.Processing.Interfaces.ACL.Services;

namespace Hampcoders.Electrolink.API.Processing.Interfaces.ACL;

public interface IProcessingContextFacade
{
    Task RequestRelayToggleAsync(
        string executionId, string deviceId, string technicianId,
        string relayState, string reason, DateTime requestedAt);

    Task<IoTContextSnapshotDto?> GetIoTContextSnapshotAsync(string propertyId);

    Task<bool> HasActiveServiceForPropertyAsync(string propertyId, string technicianId);
}
