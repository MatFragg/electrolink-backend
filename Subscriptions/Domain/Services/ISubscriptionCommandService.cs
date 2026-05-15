using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface ISubscriptionCommandService
{
    Task<Subscription> Handle(CreateSubscriptionCommand command);
    Task<string> Handle(InitiateCheckoutCommand command);
    Task<Subscription> Handle(ActivateSubscriptionCommand command);
    Task<Subscription> Handle(RecordSuccessfulRenewalCommand command);
    Task<Subscription> Handle(StartGracePeriodCommand command);
    Task<Subscription> Handle(DegradeSubscriptionCommand command);
    Task<Subscription> Handle(CancelSubscriptionCommand command);
    Task<Subscription> Handle(IncrementMonthlyRequestCounterCommand command);
    Task Handle(ResetMonthlyRequestCountersCommand command);
    Task<Subscription> Handle(UpdateBillingCycleCommand command);
}
