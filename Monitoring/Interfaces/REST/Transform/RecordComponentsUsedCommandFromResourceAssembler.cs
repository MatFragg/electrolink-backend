using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class RecordComponentsUsedCommandFromResourceAssembler
{
    public static RecordComponentsUsedCommand ToCommandFromResource(string executionId, string technicianId, RecordComponentsUsedResource resource)
    {
        var items = resource.ComponentsUsed
            .Select(c => new ComponentUsageItem(
                c.ComponentTypeId,
                c.ComponentTypeName,
                c.QuantityUsed,
                c.QuantityReserved))
            .ToList();

        return new RecordComponentsUsedCommand(
            ServiceExecutionId.From(executionId),
            TechnicianId.From(technicianId),
            items,
            resource.RecordedAt
        );
    }
}

