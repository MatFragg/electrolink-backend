namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

public record ComponentUsageItem(
    string ComponentTypeId,
    string ComponentTypeName,
    int QuantityUsed,
    int QuantityReserved
);