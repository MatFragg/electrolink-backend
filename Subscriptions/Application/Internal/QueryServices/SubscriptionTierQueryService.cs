using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionTierQueryService : ISubscriptionTierQuery
{
    private readonly ISubscriptionRepository _repository;

    public SubscriptionTierQueryService(ISubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<string?> GetTierAsync(string userId)
    {
        var subscription = await _repository.FindByUserIdAsync(userId);
        return subscription?.PlanType?.ToString();
    }
}
