namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UpdatePortfolioItemDetailsCommand(    
    Guid WorkId,
    int ProfileId, 
    string NewTitle,
    string NewDescription,
    string NewImageUrl
);