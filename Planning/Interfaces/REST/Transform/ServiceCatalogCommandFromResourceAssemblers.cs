using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class CreateServiceRecipeCommandFromResourceAssembler
{
    public static CreateServiceRecipeCommand ToCommandFromResource(
        CreateServiceRecipeResource resource,
        Guid technicianId)
    {
        if (!Enum.TryParse<ServiceCategory>(resource.ServiceCategory, true, out var category))
            throw new ArgumentException($"Invalid service category: {resource.ServiceCategory}");

        if (!Enum.TryParse<WarrantyUnit>(resource.WarrantyUnit, true, out var warrantyUnit))
            throw new ArgumentException($"Invalid warranty unit: {resource.WarrantyUnit}");

        return new CreateServiceRecipeCommand(
            resource.CatalogId,
            technicianId,
            resource.ServiceName,
            resource.ServiceDescription,
            category,
            resource.ComponentRequirements.Select(c => new ComponentRequirementDto(
                c.ComponentTypeId,
                c.ComponentTypeName,
                c.Quantity,
                c.IsRequired
            )).ToList(),
            resource.EstimatedHours,
            resource.EstimatedMinutes,
            resource.MaterialsEstimate,
            resource.LaborCost,
            resource.TotalPrice,
            resource.Currency,
            resource.WarrantyValue,
            warrantyUnit,
            resource.Prerequisites,
            resource.Deliverables
        );
    }
}

public static class UpdateServiceRecipeCommandFromResourceAssembler
{
    public static UpdateServiceRecipeCommand ToCommandFromResource(
        UpdateServiceRecipeResource resource,
        Guid recipeId,
        Guid technicianId)
    {
        if (!Enum.TryParse<WarrantyUnit>(resource.WarrantyUnit, true, out var warrantyUnit))
            throw new ArgumentException($"Invalid warranty unit: {resource.WarrantyUnit}");

        return new UpdateServiceRecipeCommand(
            recipeId,
            technicianId,
            resource.ServiceName,
            resource.ServiceDescription,
            resource.ComponentRequirements.Select(c => new ComponentRequirementDto(
                c.ComponentTypeId,
                c.ComponentTypeName,
                c.Quantity,
                c.IsRequired
            )).ToList(),
            resource.EstimatedHours,
            resource.EstimatedMinutes,
            resource.MaterialsEstimate,
            resource.LaborCost,
            resource.TotalPrice,
            resource.Currency,
            resource.WarrantyValue,
            warrantyUnit,
            resource.Prerequisites,
            resource.Deliverables
        );
    }
}

