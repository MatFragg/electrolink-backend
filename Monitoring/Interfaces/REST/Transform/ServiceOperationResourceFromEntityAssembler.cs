using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

public static class ServiceOperationResourceFromEntityAssembler
{
    public static ServiceOperationResource ToResourceFromEntity(ServiceOperation entity)
    {
        return new ServiceOperationResource(
            entity.RequestId,
            entity.CurrentStatus.ToString(),
            entity.StartedAt,
            entity.CompletedAt
        );
    }
    
}