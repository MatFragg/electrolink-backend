using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ServiceCatalogResourceFromEntityAssembler
{
    /*public static ServiceCatalogResource ToResourceFromEntity(ServiceCatalog entity, string profileId)
    {
        return new ServiceCatalogResource(
            entity.CatalogId.Value,
            entity.TechnicianId.Value,
            profileId,
            entity.Status.ToString(),
            entity.Recipes.Select(ToResourceFromEntity).ToList()
        );
    }

    public static ServiceRecipeDetailResource ToResourceFromEntity(ServiceRecipe entity)
    {
        return new ServiceRecipeDetailResource(
            entity.Id.Value,
            entity.CatalogId.Value,
            entity.TechnicianId.Value,
            entity.ServiceName,
            entity.ServiceDescription,
            entity.ServiceCategory.ToString(),
            entity.ComponentRequirements.Select(c => new ComponentRequirementResource(
                c.ComponentTypeId,
                c.ComponentTypeName,
                c.Quantity,
                c.IsRequired
            )).ToList(),
            entity.EstimatedDuration.Hours,
            entity.EstimatedDuration.TotalMinutes,
            entity.Pricing.MaterialsEstimate.Amount,
            entity.Pricing.LaborCost.Amount,
            entity.Pricing.TotalPrice.Amount,
            entity.Pricing.TotalPrice.Currency.ToString(),
            entity.WarrantyPeriod.Months,
            entity.IsActive,
            entity.TimesRequested,
            entity.Prerequisites.ToList(),
            entity.Deliverables.ToList()
        );
    }*/
    
    public static ServiceCatalogResource ToResource(ServiceCatalog catalog) =>
        new(
            catalog.CatalogId.Value,
            catalog.TechnicianId.Value,
            catalog.Status.ToString(),
            catalog.Recipes.Count,
            catalog.Recipes.Select(r => new ServiceRecipeSummaryResource(
                r.Id.Value,
                r.ServiceName,
                r.ServiceCategory.ToString(),
                r.Pricing.TotalPrice.Amount,
                r.Pricing.TotalPrice.Currency.ToString(),
                r.EstimatedDuration.TotalMinutes,
                r.IsActive,
                r.TimesRequested)).ToList());
}

