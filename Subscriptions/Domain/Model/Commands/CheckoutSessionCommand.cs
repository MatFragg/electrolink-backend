namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record CheckoutSessionRequest(
    UserId UserId,
    EmailAddress UserEmail,
    PlanId PlanId,
    Money Amount,
    Currency Currency,
    string SuccessUrl,
    string CancelUrl,
    CheckoutSessionMetadata? Metadata = null
);