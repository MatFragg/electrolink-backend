using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository, IPlanRepository planRepository) : ISubscriptionQueryService
{
    
    public async Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query)
    {
        return await subscriptionRepository.ListAsync();
    }

    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query)
    {
        return await subscriptionRepository.FindBySubscriptionIdAsync(new SubscriptionId(query.SubscriptionId));
    }

    public async Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query)
    {
        return await subscriptionRepository.FindByUserIdAsync(new UserId(query.UserId.Value));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Subscription>> Handle(GetAllActiveSubscriptionsQuery query)
    {
        return await subscriptionRepository.ListActiveAsync();
    }

    /// <inheritdoc/>
    public async Task<Benefit?> Handle(GetUserBenefitQuery query)
    {
        var userId = new UserId(query.UserId);
        var subscription = await subscriptionRepository.FindByUserIdAsync(userId); // Corrected: int UserId
        if (subscription == null)
        {
            // If no explicit subscription, consider the default freemium plan (if any)
            // This assumes a user without an active subscription defaults to a freemium experience.
            var defaultFreemiumPlan = (await planRepository.ListPlansByRoleAsync(EUserRole.All)) // Query all for All or specific role
                .FirstOrDefault(p => p.MonetizationType == EMonetizationType.Free);

            if (defaultFreemiumPlan != null)
            {
                return defaultFreemiumPlan.GetBenefit(query.BenefitType);
            }
            return null;
        }

        var plan = await planRepository.FindByIdAsync(subscription.PlanId);
        if (plan == null) return null;

        return plan.GetBenefit(query.BenefitType);
    }

    public async Task<Guid?> Handle(GetLocalSubscriptionIdQuery query)
    {
        var subscription = await subscriptionRepository.FindByPaymentGatewaySubscriptionIdAsync(new PaymentGatewaySubscriptionId(query.StripeSubscriptionId));
        return subscription?.Id.Value; // Accedes al Guid del ValueObject
    }
}