namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

/// <summary>
/// Resource representing a homeowner's property portfolio with its entries.
/// </summary>
public record PropertyPortfolioResource(
    string Id,
    string HomeownerId,
    string Status,
    List<PortfolioEntryResource> Entries);

