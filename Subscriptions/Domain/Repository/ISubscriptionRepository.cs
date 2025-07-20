using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Repository;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription?> FindByIdAsync(SubscriptionId id);
}