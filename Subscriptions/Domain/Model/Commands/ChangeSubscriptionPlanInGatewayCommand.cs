namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// LEGACY - Compatibility command. Not used in tactical design.
/// </summary>
public record ChangeSubscriptionPlanInGatewayCommand(string SubscriptionId, string NewPlanId);
