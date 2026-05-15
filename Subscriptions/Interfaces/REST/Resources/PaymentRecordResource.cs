namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record PaymentRecordResource(
    string PaymentRecordId,
    string StripeInvoiceId,
    decimal AmountDecimal,
    string Currency,
    string Status,
    string ProcessedAt);

