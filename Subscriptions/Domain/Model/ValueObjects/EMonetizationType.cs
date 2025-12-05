namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Enum for the monetization type of a plan.
/// </summary>
public enum EMonetizationType
{
    /// <summary>
    /// The plan is free.
    /// </summary>
    Free,
    /// <summary>
    /// The plan is billed monthly.
    /// </summary>
    Monthly,
    /// <summary>
    /// The plan is billed annually.
    /// </summary>
    Annually
}