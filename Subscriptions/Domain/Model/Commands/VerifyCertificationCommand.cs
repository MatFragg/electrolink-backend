namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record VerifyCertificationCommand(Guid SubscriptionId, DateTime Now);