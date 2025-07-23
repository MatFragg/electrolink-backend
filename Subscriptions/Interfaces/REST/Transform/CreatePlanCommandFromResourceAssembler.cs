using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hamcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hamcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class CreatePlanCommandFromResourceAssembler
{
    public static CreatePlanCommand ToCommand(CreatePlanResource resource)
    {
        return new CreatePlanCommand(
            resource.Name,
            resource.Description,
            resource.Price,
            resource.Currency,
            Enum.Parse<MonetizationType>(resource.MonetizationType, ignoreCase: true),
            resource.IsDefault
        );
    }
}