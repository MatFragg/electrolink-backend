﻿using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
/// Aggregate: PaymentRecord
/// 
/// Immutable, append-only aggregate recording each payment attempt from Stripe.
/// Created whenever a payment succeeds, fails, or is refunded.
/// 
/// No domain logic; purely a trace entity for audit/reporting.
/// </summary>
public class PaymentRecord
{
    public PaymentRecordId PaymentRecordId { get; private set; }
    public SubscriptionId SubscriptionId { get; private set; }
    public StripeInvoiceId StripeInvoiceId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime ProcessedAt { get; private set; }

    private PaymentRecord() { }

    private PaymentRecord(
        PaymentRecordId paymentRecordId,
        SubscriptionId subscriptionId,
        StripeInvoiceId stripeInvoiceId,
        Money amount,
        PaymentStatus status,
        DateTime processedAt)
    {
        PaymentRecordId = paymentRecordId;
        SubscriptionId = subscriptionId;
        StripeInvoiceId = stripeInvoiceId;
        Amount = amount;
        Status = status;
        ProcessedAt = processedAt;
    }

    /// <summary>
    /// Factory method to create a new payment record.
    /// </summary>
    public static PaymentRecord Create(
        SubscriptionId subscriptionId,
        StripeInvoiceId stripeInvoiceId,
        Money amount,
        PaymentStatus status,
        DateTime processedAt)
    {
        return new PaymentRecord(
            PaymentRecordId.NewPaymentRecordId(),
            subscriptionId,
            stripeInvoiceId,
            amount,
            status,
            processedAt);
    }
}

