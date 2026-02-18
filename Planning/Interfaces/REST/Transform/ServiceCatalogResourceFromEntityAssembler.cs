using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ServiceCatalogResourceFromEntityAssembler
{
    public static ServiceCatalogResource ToResourceFromEntity(ServiceCatalog entity)
    {
        return new ServiceCatalogResource(
            entity.Id.Id,
            entity.TechnicianId.Id,
            entity.Status.ToString(),
            entity.Recipes.Select(ToResourceFromEntity).ToList()
        );
    }

    public static ServiceRecipeResource ToResourceFromEntity(ServiceRecipe entity)
    {
        return new ServiceRecipeResource(
            entity.Id.Id,
            entity.CatalogId.Id,
            entity.ServiceName.Value,
            entity.ServiceDescription,
            entity.ServiceCategory.ToString(),
            entity.ComponentRequirements.Select(c => new ComponentRequirementResource(
                c.ComponentTypeId,
                c.ComponentTypeName,
                c.Quantity,
                c.IsRequired
            )).ToList(),
            entity.EstimatedDuration.TotalMinutes,
            entity.Pricing.MaterialsEstimate.Amount,
            entity.Pricing.LaborCost.Amount,
            entity.Pricing.TotalPrice.Amount,
            entity.Pricing.TotalPrice.Currency,
            entity.WarrantyPeriod.Value,
            entity.WarrantyPeriod.Unit.ToString(),
            entity.IsActive,
            entity.TimesRequested,
            entity.Prerequisites.ToList(),
            entity.Deliverables.ToList()
        );
    }
}

