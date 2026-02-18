using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ServiceResourceFromEntityAssembler
{
    public static ServiceResource ToResourceFromEntity(Service s) =>
        new ServiceResource(
            s.Id.ToString(),
            s.Name,
            s.Description,
            s.BasePrice.Amount,
            s.EstimatedTime,
            s.Category,
            s.IsVisible,
            s.CreatedBy.Id
        );
}