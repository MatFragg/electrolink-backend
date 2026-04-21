namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// LEGACY - Compatibility command. Renamed to IncrementMonthlyRequestCounterCommand in tactical design.
/// </summary>
public record IncrementSubscriptionUsageCommand(string SubscriptionId);
