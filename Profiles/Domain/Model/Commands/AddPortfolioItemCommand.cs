namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record AddPortfolioItemCommand(
    string Title,
    string Description,
    string ImageUrl,
    int ProfileId
);