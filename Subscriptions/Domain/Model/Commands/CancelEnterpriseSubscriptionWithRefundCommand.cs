namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record CancelEnterpriseSubscriptionWithRefundCommand(
    string SubscriptionId,
    string Reason,
    decimal RefundAmount,
    string Currency);
