using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ServiceAssignmentResourceFromEntityAssembler
{
    public static ServiceAssignmentResource ToResourceFromEntity(ServiceAssignment entity)
    {
        return new ServiceAssignmentResource(
            entity.Id.Id,
            entity.RequestId.Id,
            entity.TechnicianId.Id,
            entity.HomeownerId.Id,
            entity.PropertyId.Id,
            new RecipeSnapshotResource(
                entity.RecipeSnapshot.RecipeId,
                entity.RecipeSnapshot.ServiceName,
                entity.RecipeSnapshot.ServiceCategory.ToString(),
                entity.RecipeSnapshot.ComponentRequirements.Select(c => new ComponentRequirementResource(
                    c.ComponentTypeId,
                    c.ComponentTypeName,
                    c.Quantity,
                    c.IsRequired
                )).ToList(),
                entity.RecipeSnapshot.Pricing.TotalPrice.Amount,
                entity.RecipeSnapshot.Pricing.TotalPrice.Currency,
                entity.RecipeSnapshot.EstimatedDuration.TotalMinutes,
                entity.RecipeSnapshot.WarrantyPeriod.ToMonths()
            ),
            entity.ScheduledSlot.StartDateTime,
            entity.ScheduledSlot.EndDateTime,
            entity.Status.ToString(),
            entity.IsPriority
        );
    }

    public static ServiceAssignmentListResource ToListResourceFromEntity(ServiceAssignment entity)
    {
        return new ServiceAssignmentListResource(
            entity.Id.Id,
            entity.RequestId.Id,
            entity.RecipeSnapshot.ServiceName,
            entity.TechnicianId.Id,
            entity.HomeownerId.Id,
            entity.ScheduledSlot.StartDateTime,
            entity.ScheduledSlot.EndDateTime,
            entity.Status.ToString(),
            entity.IsPriority
        );
    }
}

