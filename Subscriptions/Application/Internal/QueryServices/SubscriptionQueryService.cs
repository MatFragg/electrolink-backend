using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(
    ISubscriptionRepository subscriptionRepository,
    IPaymentRecordRepository paymentRecordRepository) : ISubscriptionQueryService
{
    public async Task<Subscription> Handle(GetMySubscriptionQuery query)
        => await subscriptionRepository.FindByUserIdOrFailAsync(query.UserId);

    public async Task<RequestEligibilityResult> Handle(GetRequestEligibilityQuery query)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(query.UserId);

        if (subscription.PlanType.IsPremium)
        {
            return new RequestEligibilityResult(
                CanRequest: true,
                IsPriorityAllowed: true,
                RemainingRequests: null,
                PlanType: subscription.PlanType.ToString(),
                UpgradeRequired: false);
        }

        var hasCapacity = subscription.UsageCounters?.HasCapacity ?? false;
        return new RequestEligibilityResult(
            CanRequest: hasCapacity,
            IsPriorityAllowed: false,
            RemainingRequests: subscription.UsageCounters?.Remaining ?? 0,
            PlanType: subscription.PlanType.ToString(),
            UpgradeRequired: !hasCapacity);
    }

    public async Task<IEnumerable<PaymentRecord>> Handle(GetPaymentHistoryQuery query)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(query.UserId);
        return await paymentRecordRepository.FindBySubscriptionIdAsync(subscription.SubscriptionId.Value);
    }

    public async Task<Subscription?> Handle(GetSubscriptionStatusAlertQuery query)
    {
        var subscription = await subscriptionRepository.FindByUserIdOrFailAsync(query.UserId);
        return subscription.Status.IsInGracePeriod ? subscription : null;
    }

    public async Task<Subscription?> Handle(GetSubscriptionByStripeCustomerIdQuery query)
        => await subscriptionRepository.FindByStripeCustomerIdAsync(query.StripeCustomerId);
}