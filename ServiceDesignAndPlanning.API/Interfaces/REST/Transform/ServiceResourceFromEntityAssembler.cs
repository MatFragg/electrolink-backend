using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Transform;

public static class ServiceResourceFromEntityAssembler
{
    public static ServiceResource ToResourceFromEntity(Service s) =>
        new ServiceResource(
            s.ServiceId,
            s.Name,
            s.Description,
            s.BasePrice,
            s.EstimatedTime,
            s.Category,
            s.IsVisible,
            s.CreatedBy
        );
}