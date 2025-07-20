using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public static class CreateReportCommandFromResourceAssembler
{
    public static AddReportCommand ToCommandFromResource(Guid requestId, CreateReportResource resource)
        => new(requestId, resource.Description);
}