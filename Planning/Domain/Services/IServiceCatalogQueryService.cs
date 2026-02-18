using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IServiceCatalogQueryService
{
    Task<ServiceCatalog?> Handle(GetServiceCatalogByTechnicianQuery query);
    Task<ServiceRecipe?> Handle(GetServiceRecipeByIdQuery query);
    Task<IEnumerable<ServiceRecipe>> Handle(GetActiveRecipesByTechnicianQuery query);
    Task<IEnumerable<ServiceRecipe>> Handle(GetAllRecipesByCatalogIdQuery query);
}

