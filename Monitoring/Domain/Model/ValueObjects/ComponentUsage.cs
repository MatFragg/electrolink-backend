namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

public record ComponentUsage(
    string ComponentTypeId,
    string ComponentTypeName,
    int QuantityUsed,
    int QuantityReserved,
    int Delta
);