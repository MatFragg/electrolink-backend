using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.ACL;

public class SubscriptionContextFacade(
    ISubscriptionQueryService subscriptionQueryService,
    ISubscriptionCommandService subscriptionCommandService)
    : ISubscriptionContextFacade
{
    public async Task<bool> RecordServiceRequestUsageAsync(int ownerUserId)
    {
        var subscription = await subscriptionQueryService.Handle(
            new GetMySubscriptionQuery(ownerUserId.ToString()));

        var result = await subscriptionCommandService.Handle(
            new IncrementSubscriptionUsageCommand(subscription.Id.Value));

        return result.HasValue;
    }

    public async Task<bool> CanCreateRequestAsync(string homeownerId)
    {
        var eligibility = await subscriptionQueryService.Handle(new GetRequestEligibilityQuery(homeownerId));
        return eligibility.CanRequest;
    }

    public async Task<bool> CanMarkAsPriorityAsync(string homeownerId)
    {
        var eligibility = await subscriptionQueryService.Handle(new GetRequestEligibilityQuery(homeownerId));
        return eligibility.IsPriorityAllowed;
    }

    public async Task<int?> GetRemainingRequestsAsync(string homeownerId)
    {
        var eligibility = await subscriptionQueryService.Handle(new GetRequestEligibilityQuery(homeownerId));
        return eligibility.RemainingRequests;
    }

    public async Task<bool> IsTechnicianPremiumAsync(string technicianId)
    {
        var eligibility = await subscriptionQueryService.Handle(new GetRequestEligibilityQuery(technicianId));
        return eligibility.PlanType == "PREMIUM";
    }

    public async Task<(bool canCreate, string planType, int? remainingRequests, bool canMarkAsPriority)>
        GetRequestEligibilityAsync(string homeownerId)
    {
        var eligibility = await subscriptionQueryService.Handle(new GetRequestEligibilityQuery(homeownerId));

        return (
            eligibility.CanRequest,
            eligibility.PlanType,
            eligibility.RemainingRequests,
            eligibility.IsPriorityAllowed
        );
    }

    public async Task<bool> IncrementMonthlyRequestUsageAsync(string homeownerId)
    {
        if (!int.TryParse(homeownerId, out var ownerUserId)) return false;
        return await RecordServiceRequestUsageAsync(ownerUserId);
    }

    public async Task<bool> TechnicianHasPremiumSubscriptionAsync(string technicianId)
    {
        return await IsTechnicianPremiumAsync(technicianId);
    }

    public async Task<string?> GetTechnicianPlanTypeAsync(string technicianId)
    {
        var eligibility = await subscriptionQueryService.Handle(new GetRequestEligibilityQuery(technicianId));
        return eligibility.PlanType;
    }
}