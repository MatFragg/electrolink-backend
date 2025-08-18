namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record AddPortfolioItemCommand(
    int ProfileId,
    string Title,
    string Description,
    string ImageUrl
);