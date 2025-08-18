namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record PortfolioItemResource(
    Guid WorkId,
    string Title,
    string Description,
    string ImageUrl
);