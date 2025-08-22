using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription?> FindBySubscriptionIdAsync(SubscriptionId id);
    Task<Subscription?> FindByUserIdAsync(UserId userId);
}   