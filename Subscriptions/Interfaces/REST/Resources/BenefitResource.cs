namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

/// <summary>
/// Resource for representing a specific benefit included in a plan.
/// </summary>
/// <param name="Type">The type of the benefit (e.g., "MaxServiceRequests", "PremiumFeatures").</param>
/// <param name="LimitValue">Optional numeric limit for the benefit (e.g., 5 for MaxServiceRequests).</param>
/// <param name="FlagValue">Optional boolean flag for the benefit (e.g., true for PremiumFeatures).</param>
/// <param name="Description">A description of the benefit.</param>
public record BenefitResource(string Type, int? LimitValue, bool? FlagValue, string Description);