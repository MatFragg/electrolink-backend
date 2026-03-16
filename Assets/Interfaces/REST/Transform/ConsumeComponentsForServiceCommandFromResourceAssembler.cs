using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class ConsumeComponentsForServiceCommandFromResourceAssembler
{
    public static ConsumeComponentsForServiceCommand ToCommandFromResource(string technicianId, string serviceId)
        => new ConsumeComponentsForServiceCommand(
            TechnicianId.From(technicianId),
            ServiceId.From(serviceId));
}

