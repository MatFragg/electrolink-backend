namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record ActivateBoostCommand(Guid SubscriptionId, DateTime Now);