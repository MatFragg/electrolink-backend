using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

/// <summary>
/// Domain Interface to interact with the payment gateway (Stripe).
/// Domain defines WHAT I need, Infrastructure defines HOW.
/// </summary>
public interface IPaymentGatewayService
{
    /// <summary>
    /// Creates or returns a Customer in Stripe to a User.
    /// </summary>
    /// <param name="userId">User identifier in the system.</param>
    /// <param name="email">User email.</param>
    /// <param name="name">Full Name.</param>
    /// <returns>Stripe Customer ID (cus_xxxxx).</returns>
    Task<PaymentGatewayCustomerId> CreateOrGetCustomerAsync(int userId, string email, string name);

    /// <summary>
    /// Creates a Checkout session so the user can subscribe to a plan.
    /// </summary>
    /// <param name="customerId">Stripe Customer ID.</param>
    /// <param name="priceId">Stripe Price ID of the plan.</param>
    /// <param name="successUrl">URL success.</param>
    /// <param name="cancelUrl">URL redirects if it is canceled.</param>
    /// <param name="trialPeriodDays">Trial Days (optional).</param>
    /// <returns>Session Checkout URL.</returns>
    Task<string> CreateCheckoutSessionAsync(
        PaymentGatewayCustomerId customerId,
        PaymentGatewayPriceId priceId,
        string successUrl,
        string cancelUrl,
        int? trialPeriodDays = null);

    /// <summary>
    /// Retrieves information about a subscription from Stripe.
    /// </summary>
    /// <param name="subscriptionId">Stripe Subscription ID.</param>
    /// <returns>Subscription information or null if it doesn't exist.</returns>
    Task<SubscriptionInfo?> GetSubscriptionAsync(PaymentGatewaySubscriptionId subscriptionId);

    /// <summary>
    /// Cancels a subscription in Stripe at the end of the current period.
    /// </summary>
    /// <param name="subscriptionId">Stripe Subscription ID.</param>
    /// <returns>Effective cancellation date.</returns>
    Task<DateTime> CancelSubscriptionAtPeriodEndAsync(PaymentGatewaySubscriptionId subscriptionId);

    /// <summary>
    /// Cancels a subscription in Stripe immediately.
    /// </summary>
    /// <param name="subscriptionId">Stripe Subscription ID.</param>
    Task CancelSubscriptionImmediatelyAsync(PaymentGatewaySubscriptionId subscriptionId);

    /// <summary>
    /// Changes the plan of an existing subscription (upgrade/downgrade).
    /// </summary>
    /// <param name="subscriptionId">Stripe Subscription ID.</param>
    /// <param name="newPriceId">New Stripe Price ID.</param>
    /// <param name="prorationBehavior">How to handle proration (create_prorations, none, always_invoice).</param>
    /// <returns>New subscription information.</returns>
    Task<SubscriptionInfo> UpdateSubscriptionPlanAsync(
        PaymentGatewaySubscriptionId subscriptionId,
        PaymentGatewayPriceId newPriceId,
        string prorationBehavior = "create_prorations");

    /// <summary>
    /// Retrying a failed payment manually.
    /// </summary>
    /// <param name="invoiceId">Stripe Invoice ID.</param>
    Task<bool> RetryPaymentAsync(string invoiceId);

    /// <summary>
    /// Creates a billing portal for the user to manage their subscription.
    /// </summary>
    /// <param name="customerId">Stripe Customer ID.</param>
    /// <param name="returnUrl">URL to return to after managing.</param>
    /// <returns>Billing portal URL.</returns>
    Task<string> CreateBillingPortalSessionAsync(PaymentGatewayCustomerId customerId, string returnUrl);
}
