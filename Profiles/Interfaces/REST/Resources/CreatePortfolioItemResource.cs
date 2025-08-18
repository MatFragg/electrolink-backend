namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

public record CreatePortfolioItemResource(
    string Title,
    string Description,
    string ImageUrl
);