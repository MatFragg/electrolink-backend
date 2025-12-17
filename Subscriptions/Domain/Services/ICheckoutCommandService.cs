using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface ICheckoutCommandService
{
    Task<string> Handle(CreateCheckoutSessionCommand command);
    Task Handle(CancelSubscriptionInGatewayCommand command);
    Task Handle(ChangeSubscriptionPlanInGatewayCommand command);
}