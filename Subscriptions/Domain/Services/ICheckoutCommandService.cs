using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface ICheckoutCommandService
{
    Task<string> Handle(CreateCheckoutSessionCommand command);
    Task Handle(CancelSubscriptionCommand command);
    Task Handle(ChangeSubscriptionPlanInGatewayCommand command);
}