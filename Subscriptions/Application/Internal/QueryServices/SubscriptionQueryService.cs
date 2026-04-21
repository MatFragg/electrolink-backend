using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(
    ISubscriptionRepository subscriptionRepository,
    IPaymentRecordRepository paymentRecordRepository) : ISubscriptionQueryService
{
    public async Task<Subscription> Handle(GetMySubscriptionQuery query)
    {
        var userId = ParseUserId(query.UserId);
        return await subscriptionRepository.FindByUserIdAsync(userId)
               ?? throw new ArgumentException($"No subscription found for user {query.UserId}.");
    }

    public async Task<RequestEligibilityResult> Handle(GetRequestEligibilityQuery query)
    {
        var userId = ParseUserId(query.UserId);
        var subscription = await subscriptionRepository.FindByUserIdAsync(userId)
                           ?? throw new ArgumentException($"No subscription found for user {query.UserId}.");

        if (subscription.PlanType.IsPremium)
            return new RequestEligibilityResult(true, true, null, subscription.PlanType.ToString(), false);

        var hasCapacity = subscription.UsageCounters?.HasCapacity ?? false;
        return new RequestEligibilityResult(
            hasCapacity,
            false,
            subscription.UsageCounters?.Remaining ?? 0,
            subscription.PlanType.ToString(),
            !hasCapacity);
    }

    public async Task<IEnumerable<PaymentRecord>> Handle(GetPaymentHistoryQuery query)
    {
        var subscription = await Handle(new GetMySubscriptionQuery(query.UserId));
        return await paymentRecordRepository.FindBySubscriptionIdAsync(subscription.Id);
    }

    public async Task<Subscription?> Handle(GetSubscriptionStatusAlertQuery query)
    {
        var subscription = await Handle(new GetMySubscriptionQuery(query.UserId));
        return subscription.Status == ESubscriptionStatus.GracePeriod ? subscription : null;
    }

    public async Task<Subscription?> Handle(GetSubscriptionByStripeCustomerIdQuery query)
    {
        var customerId = new PaymentGatewayCustomerId(query.StripeCustomerId);
        return await subscriptionRepository.FindByPaymentGatewayCustomerIdAsync(customerId);
    }

    private static UserId ParseUserId(string userId)
    {
        if (!int.TryParse(userId, out var value))
            throw new ArgumentException($"Invalid user id format: {userId}");

        return new UserId(value);
    }
}