using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class AddPortfolioItemCommandFromResourceAssembler
{
    public static AddPortfolioItemCommand ToCommandFromResource(int profileId, CreatePortfolioItemResource resource)
    {
        return new AddPortfolioItemCommand(resource.Title, resource.Description, resource.ImageUrl, profileId);
    }
}