using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ServiceRecipeDetailResourceFromEntityAssembler
{
    public static ServiceRecipeDetailResource ToResource(ServiceRecipe recipe) =>
        new(
            recipe.Id.Value,
            recipe.ServiceName,
            recipe.ServiceDescription,
            recipe.ServiceCategory.ToString(),
            recipe.ComponentRequirements.Select(c =>
                new ComponentRequirementResource(
                    c.ComponentTypeId, c.Quantity, c.IsRequired)).ToList(),
            recipe.EstimatedDuration.TotalMinutes,
            recipe.Pricing.MaterialsEstimate.Amount,
            recipe.Pricing.LaborCost.Amount,
            recipe.Pricing.TotalPrice.Amount,
            recipe.Pricing.TotalPrice.Currency.ToString(),
            recipe.Prerequisites.ToList(),
            recipe.Deliverables.ToList(),
            recipe.WarrantyPeriod.Months,
            recipe.IsActive,
            recipe.TimesRequested);
}