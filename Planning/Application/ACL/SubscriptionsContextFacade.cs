using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Interfaces.ACL;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.Application.ACL;

/// <summary>
/// Implementation of the facade for accessing Subscriptions bounded context.
/// Translates Planning BC requests to Subscriptions BC queries.
/// </summary>
public class SubscriptionsContextFacade(
    ISubscriptionQueryService subscriptionQueryService,
    IPlanQueryService planQueryService,
    IRequestRepository requestRepository) : ISubscriptionsContextFacade
{
    public async Task<SubscriptionPlanInfo?> GetSubscriptionPlanByClientIdAsync(ClientId clientId)
    {
        // Note: ClientId in Planning maps to UserId in Subscriptions
        // Using the Guid from ClientId as the user profile ID
        // TODO: This may need adjustment based on actual ID mapping between contexts
        var query = new GetSubscriptionByUserIdQuery(new UserId(clientId.Id)); 
        var subscription = await subscriptionQueryService.Handle(query);

        if (subscription == null) 
            return null;

        var planQuery = new GetPlanByIdQuery(subscription.PlanId.Value);
        var plan = await planQueryService.Handle(planQuery);

        if (plan == null) 
            return null;

        // Assuming basic plan has name "Basic" or similar identifier
        bool isBasicPlan = plan.Name.Contains("Basic", StringComparison.OrdinalIgnoreCase);
        int monthlyLimit = isBasicPlan ? 3 : int.MaxValue; // Basic = 3, Premium = unlimited

        return new SubscriptionPlanInfo(
            plan.Name,
            monthlyLimit,
            isBasicPlan);
    }

    public async Task<bool> IsMonthlyLimitReachedAsync(ClientId clientId, int year, int month)
    {
        var planInfo = await GetSubscriptionPlanByClientIdAsync(clientId);
        
        if (planInfo == null)
            return true; // No subscription = can't create requests

        if (!planInfo.IsBasicPlan)
            return false; // Premium users have no limit

        var currentMonthCount = await requestRepository.CountByClientIdAndMonthAsync(clientId, year, month);
        return currentMonthCount >= planInfo.MonthlyRequestLimit;
    }
}

