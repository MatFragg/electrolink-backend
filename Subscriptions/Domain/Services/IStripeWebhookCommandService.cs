using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Stripe;
namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface IStripeWebhookCommandService
{
    //Task HandleWebhookEventAsync(Event stripeEvent);
    Task Handle(SyncSubscriptionFromGatewayCommand command);
}