using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Services;

namespace Hamcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository _subscriptionRepository) : ISubscriptionQueryService
{
    public async Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query)
    {
        return await _subscriptionRepository.ListAsync();
    }

    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query)
    {
        return await _subscriptionRepository.FindByIdAsync(new SubscriptionId(query.SubscriptionId));
    }
}