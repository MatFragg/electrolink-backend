namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

public record CreateSubscriptionCommand(int  UserId, Guid PlanId);