namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

/// <summary>
/// Resource for adding a property to a portfolio.
/// </summary>
public record AddPropertyToPortfolioResource(
    string PropertyId,
    string Nickname,
    bool IsPrimary,
    string OccupancyStatus);

