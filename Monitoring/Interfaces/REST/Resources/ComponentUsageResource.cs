namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record ComponentUsageResource(
    string ComponentTypeId,
    int QuantityUsed,
    int QuantityReserved,
    int Delta
);