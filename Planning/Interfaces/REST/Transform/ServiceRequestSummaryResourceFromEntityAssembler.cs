using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ServiceRequestSummaryResourceFromEntityAssembler
{
    public static ServiceRequestSummaryResource ToResource(ServiceRequest request) =>
        new(
            request.RequestId.Value,
            request.HomeownerId.Value,
            request.PropertyId?.Value,
            request.SelectedRecipeId?.Value,
            request.SelectedTechnicianId?.Value,
            request.Status.ToString(),
            request.IsPriority,
            request.CreatedDate.ToString());
}
