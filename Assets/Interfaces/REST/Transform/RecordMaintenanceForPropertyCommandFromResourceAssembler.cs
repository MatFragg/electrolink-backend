using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class RecordMaintenanceForPropertyCommandFromResourceAssembler
{
    public static RecordMaintenanceForPropertyCommand ToCommandFromResource(RecordMaintenanceResource resource, string propertyId)
        => new RecordMaintenanceForPropertyCommand(
            PropertyId.From(propertyId),
            AssignmentId.From(resource.ServiceId),
            TechnicianId.From(resource.TechnicianId),
            resource.WorkSummary,
            resource.CompletedAt);
}

