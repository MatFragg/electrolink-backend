using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.ACL.Services;

public class SubscriptionsContextFacade : ISubscriptionsContextFacade
{
    private readonly ISubscriptionQueryService _queryService;

    public SubscriptionsContextFacade(ISubscriptionQueryService queryService)
        => _queryService = queryService;

    public async Task<bool> IsUserPremiumAsync(string userId)
    {
        var subscription = await _queryService.Handle(new GetMySubscriptionQuery(userId));
        return subscription.PlanType.IsPremium && subscription.Status.IsActive;
    }

    public async Task<RequestEligibilityResult> GetRequestEligibilityAsync(string userId)
        => await _queryService.Handle(new GetRequestEligibilityQuery(userId));
}
