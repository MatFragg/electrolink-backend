using Hampcoders.Electrolink.API.Subscriptions.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

/// <summary>
/// </summary>
public class ExternalSubscriptionsService(ISubscriptionContextFacade subscriptionContextFacade)
{
    /// <summary>
    /// </summary>
    public async Task<bool> CanCreateRequestAsync(string homeownerId)
    {
        var eligibility = await subscriptionContextFacade.GetRequestEligibilityAsync(homeownerId);
        return eligibility.canCreate;
    }

    /// <summary>
    /// </summary>
    public async Task<string> GetPlanTypeAsync(string homeownerId)
    {
        var eligibility = await subscriptionContextFacade.GetRequestEligibilityAsync(homeownerId);
        return eligibility.planType;
    }

    /// <summary>
    /// </summary>
    public async Task<(bool canCreate, string planType, int? remainingRequests, bool canMarkAsPriority)> GetRemainingRequestsAsync(string homeownerId) 
        => await subscriptionContextFacade.GetRequestEligibilityAsync(homeownerId);

    /// <summary>
    /// </summary>
    public async Task<bool> CanMarkAsPriorityAsync(string homeownerId)
    {
        var eligibility = await subscriptionContextFacade.GetRequestEligibilityAsync(homeownerId);
        return eligibility.canMarkAsPriority;
    }
}

