namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record GrantPremiumAccessCommand(Guid SubscriptionId, DateTime ValidUntil);