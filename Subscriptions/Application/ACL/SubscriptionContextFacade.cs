using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.ACL;

public class SubscriptionContextFacade(
    ISubscriptionQueryService subscriptionQueryService,
    ISubscriptionCommandService subscriptionCommandService)
    : ISubscriptionContextFacade
{
    public Task<bool> RecordServiceRequestUsageAsync(int ownerUserId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CanCreateRequestAsync(string homeownerId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CanMarkAsPriorityAsync(string homeownerId)
    {
        throw new NotImplementedException();
    }

    public Task<int?> GetRemainingRequestsAsync(string homeownerId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsTechnicianPremiumAsync(string technicianId)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool canCreate, string planType, int? remainingRequests, bool canMarkAsPriority)>
        GetRequestEligibilityAsync(string homeownerId)
    {
        /*var result = await subscriptionQueryService.Handle(
            new GetHomeownerEligibilityQuery(homeownerId));

        return (
            result.CanCreate,
            result.PlanType,
            result.RemainingRequests,
            result.CanMarkAsPriority
        );*/
        return (true, "PREMIUM", null, true);
    }

    public async Task<bool> IncrementMonthlyRequestUsageAsync(string homeownerId)
    {
        /*var result = await subscriptionCommandService.Handle(
            new IncrementMonthlyUsageCommand(homeownerId));
        return result;*/ 
        return true;
    }

    public async Task<bool> TechnicianHasPremiumSubscriptionAsync(string technicianId)
    {
        /*var plan = await subscriptionQueryService.Handle(
            new GetTechnicianPlanQuery(technicianId));
        return plan?.IsActive == true && plan.PlanType == "PREMIUM";*/
        return true;
    }

    public async Task<string?> GetTechnicianPlanTypeAsync(string technicianId)
    {
        /*var plan = await subscriptionQueryService.Handle(
            new GetTechnicianPlanQuery(technicianId));
        return plan?.PlanType;*/
        return "PREMIUM";
    }
}