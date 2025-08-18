using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

public static class PortfolioItemResourceFromEntityAssembler
{
    public static PortfolioItemResource ToResourceFromEntity(PortfolioItem entity)
    {
        return new PortfolioItemResource(entity.WorkId, entity.Title, entity.Description, entity.ImageUrl);
    }
}