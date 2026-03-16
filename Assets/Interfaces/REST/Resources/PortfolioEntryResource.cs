namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

/// <summary>
/// Resource representing a single entry in a property portfolio.
/// </summary>
public record PortfolioEntryResource(
    string PropertyId,
    string Nickname,
    bool IsPrimary,
    string OccupancyStatus);

