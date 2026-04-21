using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

/// <summary>
/// Assembler to convert CreateCheckoutSessionResource to CreateCheckoutSessionCommand.
/// </summary>
public static class CreateCheckoutSessionCommandFromResourceAssembler
{
    public static CreateCheckoutSessionCommand ToCommandFromResource(string userId, CreateCheckoutSessionResource resource)
    {
        return new CreateCheckoutSessionCommand(
            userId,
            new PaymentGatewayPriceId(resource.PriceId),
            resource.SuccessUrl,
            resource.CancelUrl,
            resource.TrialPeriodDays
        );
    }
}