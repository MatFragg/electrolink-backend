namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command: Update billing cycle (Monthly ↔ Annual).
/// Webhook: customer.subscription.updated
/// </summary>
public record UpdateBillingCycleCommand(
    string StripeSubscriptionId,
    string NewBillingCycle,
    DateTime NewPeriodStart,
    DateTime NewPeriodEnd);
