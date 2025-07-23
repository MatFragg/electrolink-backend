using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class UpdatePlanCommandFromResourceAssembler
{
    public static UpdatePlanCommand ToCommand(Guid id, UpdatePlanResource resource)
    {
        return new UpdatePlanCommand(
            id,
            resource.Name,
            resource.Description,
            resource.Price,
            resource.Currency,
            resource.MonetizationType,
            resource.IsDefault
        );
    }
}