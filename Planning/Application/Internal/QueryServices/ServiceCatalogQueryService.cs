using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;

public class ServiceCatalogQueryService(
    IServiceCatalogRepository catalogRepository,
    ILogger<ServiceCatalogQueryService> logger)
    : IServiceCatalogQueryService
{
    public async Task<ServiceCatalog?> Handle(GetServiceCatalogByTechnicianQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting catalog for technician {query.TechnicianId}");
        return await catalogRepository.FindByTechnicianIdAsync(new TechnicianId(query.TechnicianId));
    }

    public async Task<ServiceRecipe?> Handle(GetServiceRecipeByIdQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting recipe {query.RecipeId}");
        
        // Buscar en todos los catálogos
        var catalogs = await catalogRepository.FindAllAsync();
        
        foreach (var catalog in catalogs)
        {
            var recipe = catalog.Recipes.FirstOrDefault(r => r.Id.Id == query.RecipeId);
            if (recipe != null)
                return recipe;
        }
        
        return null;
    }

    public async Task<IEnumerable<ServiceRecipe>> Handle(GetActiveRecipesByTechnicianQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting active recipes for technician {query.TechnicianId}");
        
        var catalog = await catalogRepository.FindByTechnicianIdAsync(new TechnicianId(query.TechnicianId));
        
        if (catalog == null)
            return Enumerable.Empty<ServiceRecipe>();
        
        return catalog.Recipes.Where(r => r.IsActive).ToList();
    }

    public async Task<IEnumerable<ServiceRecipe>> Handle(GetAllRecipesByCatalogIdQuery query)
    {
        logger.LogInformation($"[Planning BC] Query: Getting all recipes for catalog {query.CatalogId}");
        
        var catalog = await catalogRepository.FindByIdAsync(new CatalogId(query.CatalogId));
        
        if (catalog == null)
            return Enumerable.Empty<ServiceRecipe>();
        
        return catalog.Recipes.ToList();
    }
}

