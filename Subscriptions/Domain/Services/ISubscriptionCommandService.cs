using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface ISubscriptionCommandService
{
    Task<Guid> Handle(CreateSubscriptionCommand command);
    Task<Guid?> Handle(UpdateSubscriptionStatusCommand command);
    Task<Guid?> Handle(IncrementSubscriptionUsageCommand command);
    Task<Guid?> Handle(ChangeSubscriptionPlanCommand command);
    Task<string> Handle(CreateCheckoutSessionCommand command);
    Task Handle(CancelSubscriptionCommand command);
    Task Handle(ChangeSubscriptionPlanInGatewayCommand command);
    Task Handle(SyncSubscriptionFromGatewayCommand command);
    Task<string> Handle(CreateBillingPortalSessionCommand command);

    Task Handle(ActivateSubscriptionCommand command);
    Task Handle(RecordSuccessfulRenewalCommand command);
    Task Handle(StartGracePeriodCommand command);
    Task Handle(DegradeSubscriptionCommand command);
    Task Handle(UpdateBillingCycleCommand command);
    Task Handle(ResetMonthlyRequestCountersCommand command);
}
