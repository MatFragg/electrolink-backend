using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record CheckoutSessionCommand(
    UserId UserId,
    string UserEmail,
    PaymentGatewayPriceId PriceId,
    decimal Amount,
    string Currency,
    string SuccessUrl,
    string CancelUrl,
    CheckoutSessionMetadata? Metadata = null
);