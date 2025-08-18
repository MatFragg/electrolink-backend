namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record RemovePortfolioItemCommand(int ProfileId, 
    Guid WorkId);