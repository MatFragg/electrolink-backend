using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class CancelSubscriptionCommandFromResourceAssembler
{
    public static CancelSubscriptionCommand ToCommandFromResource(Guid subscriptionId, CancelSubscriptionResource resource)
    {
        return new CancelSubscriptionCommand(
            new SubscriptionId(subscriptionId),
            resource.Immediately
        );
    }
}