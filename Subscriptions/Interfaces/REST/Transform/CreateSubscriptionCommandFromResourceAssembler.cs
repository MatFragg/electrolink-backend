using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class CreateSubscriptionCommandFromResourceAssembler
{
    public static CreateSubscriptionCommand ToCommand(CreateSubscriptionResource resource)
    {
        return new CreateSubscriptionCommand(resource.UserId, resource.PlanId);
    }
}