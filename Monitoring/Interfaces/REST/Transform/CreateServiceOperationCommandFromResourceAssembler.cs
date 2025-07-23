using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class CreateServiceOperationCommandFromResourceAssembler
{
    public static CreateServiceOperationCommand ToCommandFromResource(CreateServiceOperationResource resource)
        => new(resource.RequestId, resource.TechnicianId);
}