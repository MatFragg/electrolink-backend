using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class ReleaseReservationCommandFromResourceAssembler
{
    public static ReleaseReservationCommand ToCommandFromResource(string technicianId, string serviceId, string reason)
        => new ReleaseReservationCommand(
            TechnicianId.From(technicianId),
            AssignmentId.From(serviceId),
            reason);
}

