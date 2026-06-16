using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IServiceCatalogRepository : IBaseRepository<ServiceCatalog, CatalogId>
{
    Task<ServiceCatalog?> FindByTechnicianIdAsync(TechnicianId technicianId);
    Task<ServiceCatalog?> FindByCatalogIdAsync(CatalogId catalogId);
    Task<bool> ExistsByTechnicianIdAsync(TechnicianId technicianId);
    Task<ServiceRecipe?> FindActiveRecipeByIdAsync(RecipeId recipeId);
    Task<ServiceRecipe?> FindActiveRecipeByCategoryAndTechnicianAsync(EServiceCategory serviceCategory, TechnicianId technicianId);
    Task<Dictionary<TechnicianId, ServiceRecipe>> FindActiveRecipesByCategoryAndTechnicianIdsAsync(
        EServiceCategory serviceCategory, IEnumerable<TechnicianId> technicianIds);
    Task<Dictionary<TechnicianId, ServiceCatalog>> FindCatalogsByTechnicianIdsAsync(
        IEnumerable<TechnicianId> technicianIds);
}
