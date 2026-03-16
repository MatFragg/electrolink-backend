using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class ReserveComponentsCommandFromResourceAssembler
{
    public static ReserveComponentsForServiceCommand ToCommandFromResource(
        ReserveComponentsResource resource,
        string technicianId)
    {
        var adjustments = resource.Components
            .Select(c => new ComponentAdjustment(ComponentId.From(c.ComponentId), c.Quantity))
            .ToList()
            .AsReadOnly();

        return new ReserveComponentsForServiceCommand(
            TechnicianId.From(technicianId),
            ServiceId.From(resource.ServiceId),
            adjustments);
    }
}

