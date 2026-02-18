using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

public partial class ServiceCatalog
{
    public ServiceRecipe AddRecipe(CreateServiceRecipeCommand command)
    {
        // Validar unicidad de nombre
        if (_recipes.Any(r => r.ServiceName.Value.Equals(command.ServiceName, StringComparison.OrdinalIgnoreCase)))
            throw new DuplicateRecipeNameException(command.ServiceName);
        
        var recipe = new ServiceRecipe(command);
        _recipes.Add(recipe);
        
        // Transición de estado si es primer recipe
        if (Status == CatalogStatus.Empty)
            Status = CatalogStatus.Active;
        
        _domainEvents.Add(new ServiceRecipeCreatedEvent(
            recipe.Id.Id,
            Id.Id,
            TechnicianId.Id,
            recipe.ServiceName.Value,
            recipe.ServiceCategory.ToString(),
            recipe.Pricing.TotalPrice.Amount,
            recipe.Pricing.TotalPrice.Currency,
            DateTime.UtcNow
        ));
        
        return recipe;
    }
    
    public void UpdateRecipe(RecipeId recipeId, UpdateServiceRecipeCommand command)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id.Equals(recipeId))
            ?? throw new RecipeNotFoundException(recipeId.Id);
        
        recipe.Update(command);
        
        _domainEvents.Add(new ServiceRecipeUpdatedEvent(
            recipeId.Id,
            Id.Id,
            DateTime.UtcNow
        ));
    }
    
    public void DeactivateRecipe(RecipeId recipeId, string reason)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id.Equals(recipeId))
            ?? throw new RecipeNotFoundException(recipeId.Id);
        
        recipe.Deactivate(reason);
        
        // Si era el último recipe activo, cambiar status del catálogo
        if (_recipes.All(r => !r.IsActive))
            Status = CatalogStatus.Inactive;
        
        _domainEvents.Add(new ServiceRecipeDeactivatedEvent(
            recipeId.Id,
            Id.Id,
            reason,
            DateTime.UtcNow
        ));
    }
    
    public void ReactivateRecipe(RecipeId recipeId)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id.Equals(recipeId))
            ?? throw new RecipeNotFoundException(recipeId.Id);
        
        recipe.Reactivate();
        
        if (Status == CatalogStatus.Inactive)
            Status = CatalogStatus.Active;
        
        _domainEvents.Add(new ServiceRecipeReactivatedEvent(
            recipeId.Id,
            Id.Id,
            DateTime.UtcNow
        ));
    }
}

