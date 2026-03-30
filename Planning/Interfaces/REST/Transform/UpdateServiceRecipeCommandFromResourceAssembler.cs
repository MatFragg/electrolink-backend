using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class UpdateServiceRecipeCommandFromResourceAssembler
{
    public static UpdateServiceRecipeCommand ToCommandFromResource(
        UpdateServiceRecipeResource resource,
        string recipeId,
        string catalogId,
        string technicianId)
    {
        return new UpdateServiceRecipeCommand(
            CatalogId.From(catalogId),
            RecipeId.From(recipeId),
            TechnicianId.From(technicianId),
            resource.ServiceName,
            resource.ServiceDescription,
            resource.ComponentRequirements?.Select(c => ComponentRequirementItem.Create(
                c.ComponentTypeId,
                string.Empty,
                c.Quantity,
                c.IsRequired
            )).ToList(),
            resource.EstimatedDurationHours,
            resource.EstimatedDurationMinutes,
            resource.Pricing?.MaterialsEstimate,
            resource.Pricing?.LaborCost,
            resource.Pricing?.TotalPrice,
            resource.Pricing?.Currency,
            resource.Prerequisites,
            resource.Deliverables,
            resource.WarrantyMonths
        );
    }
}
