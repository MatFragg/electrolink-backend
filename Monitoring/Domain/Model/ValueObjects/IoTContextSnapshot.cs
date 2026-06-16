using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

public record IoTContextSnapshot(
    DeviceId DeviceId,
    ERelayState RelayState,
    DateTime Timestamp
);
