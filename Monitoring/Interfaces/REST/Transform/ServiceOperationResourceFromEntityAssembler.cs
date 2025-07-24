using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;

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