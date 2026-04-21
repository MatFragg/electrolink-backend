using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface IPaymentGatewayService
{
    Task<PaymentGatewayCustomerId> CreateOrGetCustomerAsync(int userId, string email, string name);
    Task<string> CreateCheckoutSessionAsync(
        PaymentGatewayCustomerId customerId,
        PaymentGatewayPriceId priceId,
        string successUrl,
        string cancelUrl,
        int? trialPeriodDays = null);
    Task<SubscriptionInfo?> GetSubscriptionAsync(PaymentGatewaySubscriptionId subscriptionId);
    Task<DateTime> CancelSubscriptionAtPeriodEndAsync(PaymentGatewaySubscriptionId subscriptionId);
    Task CancelSubscriptionImmediatelyAsync(PaymentGatewaySubscriptionId subscriptionId);
    Task<SubscriptionInfo> UpdateSubscriptionPlanAsync(
        PaymentGatewaySubscriptionId subscriptionId,
        PaymentGatewayPriceId newPriceId,
        string prorationBehavior = "create_prorations");
    Task<bool> RetryPaymentAsync(string invoiceId);
    Task<string> CreateBillingPortalSessionAsync(PaymentGatewayCustomerId customerId, string returnUrl);
}

