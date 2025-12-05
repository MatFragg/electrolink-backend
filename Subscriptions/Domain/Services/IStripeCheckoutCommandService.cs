using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface IStripeCheckoutCommandService
{
    Task<string> Handle(CreateCheckoutSessionCommand command);
    Task Handle(CancelSubscriptionInStripeCommand command);
    Task Handle(ChangeSubscriptionPlanInStripeCommand command);
}