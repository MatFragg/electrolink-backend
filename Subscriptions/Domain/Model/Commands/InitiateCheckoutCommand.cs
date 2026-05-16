namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record InitiateCheckoutCommand(
    string UserId,
    string PlanType,
    string BillingCycle,
    string SuccessUrl,
    string CancelUrl);
