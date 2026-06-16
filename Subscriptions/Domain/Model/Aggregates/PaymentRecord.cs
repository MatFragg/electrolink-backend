using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

public class PaymentRecord
{
    public PaymentRecordId PaymentRecordId { get; private set; }
    public SubscriptionId SubscriptionId { get; private set; }
    public StripeInvoiceId StripeInvoiceId { get; private set; }
    public StripePaymentIntentId? StripePaymentIntentId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime ProcessedAt { get; private set; }

    private PaymentRecord() { }

    private PaymentRecord(
        PaymentRecordId paymentRecordId,
        SubscriptionId subscriptionId,
        StripeInvoiceId stripeInvoiceId,
        StripePaymentIntentId? stripePaymentIntentId,
        Money amount,
        PaymentStatus status,
        DateTime processedAt)
    {
        PaymentRecordId = paymentRecordId;
        SubscriptionId = subscriptionId;
        StripeInvoiceId = stripeInvoiceId;
        StripePaymentIntentId = stripePaymentIntentId;
        Amount = amount;
        Status = status;
        ProcessedAt = processedAt;
    }

    public static PaymentRecord Create(
        SubscriptionId subscriptionId,
        StripeInvoiceId stripeInvoiceId,
        Money amount,
        PaymentStatus status,
        DateTime processedAt,
        StripePaymentIntentId? stripePaymentIntentId = null)
    {
        return new PaymentRecord(
            PaymentRecordId.NewPaymentRecordId(),
            subscriptionId,
            stripeInvoiceId,
            stripePaymentIntentId,
            amount,
            status,
            processedAt);
    }
}
