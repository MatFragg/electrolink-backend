namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Represents a specific benefit included in a subscription plan.
/// </summary>
public record Benefit(string Type, int? LimitValue, bool? FlagValue, string Description);