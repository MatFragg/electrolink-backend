using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface ISubscriptionQueryService
{
    Task<Subscription> Handle(GetMySubscriptionQuery query);
    Task<RequestEligibilityResult> Handle(GetRequestEligibilityQuery query);
    Task<IEnumerable<PaymentRecord>> Handle(GetPaymentHistoryQuery query);
    Task<Subscription?> Handle(GetSubscriptionStatusAlertQuery query);
    Task<Subscription?> Handle(GetSubscriptionByStripeCustomerIdQuery query);
}
