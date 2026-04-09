using System;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

public class ComponentUsageRecord
{
    public ComponentUsageId Id { get; private set; }
    public ServiceExecutionId ExecutionId { get; private set; }
    public string ComponentTypeId { get; private set; }
    public string ComponentTypeName { get; private set; }
    public int QuantityUsed { get; private set; }
    public int QuantityReserved { get; private set; }

    public int Delta => QuantityUsed - QuantityReserved;

    private ComponentUsageRecord() { }

    public static ComponentUsageRecord Create(
        ComponentUsageId id,
        ServiceExecutionId executionId,
        string componentTypeId,
        string componentTypeName,
        int quantityUsed,
        int quantityReserved)
    {
        if (string.IsNullOrWhiteSpace(componentTypeId))
            throw new ArgumentException("ComponentTypeId cannot be empty.", nameof(componentTypeId));
        if (quantityUsed < 0)
            throw new ArgumentException("QuantityUsed cannot be negative.", nameof(quantityUsed));

        return new ComponentUsageRecord
        {
            Id = id,
            ExecutionId = executionId,
            ComponentTypeId = componentTypeId,
            ComponentTypeName = componentTypeName,
            QuantityUsed = quantityUsed,
            QuantityReserved = quantityReserved,
        };
    }
}
