using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class UpdatePortfolioItemDetailsCommandFromResourceAssembler
{
    public static UpdatePortfolioItemDetailsCommand ToCommandFromResource(int profileId, Guid workId, UpdatePortfolioItemResource resource)
    {
        return new UpdatePortfolioItemDetailsCommand(workId, profileId,  resource.Title, resource.Description, resource.ImageUrl);
    }
}
