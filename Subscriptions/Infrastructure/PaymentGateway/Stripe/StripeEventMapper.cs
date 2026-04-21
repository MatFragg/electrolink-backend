﻿using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;

/// <summary>
/// Maps Stripe-specific types to domain types.
/// This is the CORRECT place for Stripe-specific logic.
/// </summary>
public class StripeEventMapper
{
    public PaymentGatewayCustomerId MapCustomerId(string stripeCustomerId)
    {
        return new PaymentGatewayCustomerId(stripeCustomerId);
    }

    public PaymentGatewaySubscriptionId MapSubscriptionId(string stripeSubscriptionId)
    {
        return new PaymentGatewaySubscriptionId(stripeSubscriptionId);
    }

    public PaymentGatewayPriceId MapPriceId(string stripePriceId)
    {
        return new PaymentGatewayPriceId(stripePriceId);
    }

    public SubscriptionInfo MapToSubscriptionInfo(global::Stripe.Subscription stripeSubscription)
    {
        return new SubscriptionInfo(
            stripeSubscription.Id,
            stripeSubscription.CustomerId,
            stripeSubscription.Items.Data[0].Price.Id,
            stripeSubscription.Status,
            stripeSubscription.StartDate,
             (stripeSubscription.Items.Data.FirstOrDefault()?.CurrentPeriodEnd ?? DateTimeOffset.MinValue).UtcDateTime,
            stripeSubscription.TrialEnd,
            stripeSubscription.CanceledAt,
            stripeSubscription.CancelAt,
            stripeSubscription.CancelAtPeriodEnd,
            (stripeSubscription.Items.Data[0].Price.UnitAmount ?? 0) / 100m,
            stripeSubscription.Items.Data[0].Price.Currency
        );
    }

    public ESubscriptionStatus MapStatus(string stripeStatus)
    {
        return stripeStatus.ToLowerInvariant() switch
        {
            "active" => ESubscriptionStatus.Active,
            "trialing" => ESubscriptionStatus.Trial,
            "past_due" => ESubscriptionStatus.GracePeriod,
            "canceled" => ESubscriptionStatus.Cancelled,
            "unpaid" => ESubscriptionStatus.Expired,
            "incomplete" => ESubscriptionStatus.Pending,
            "incomplete_expired" => ESubscriptionStatus.Expired,
            _ => ESubscriptionStatus.Pending
        };
    }
}