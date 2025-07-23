using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface ISubscriptionQueryService
{
    Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query);
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query);
}