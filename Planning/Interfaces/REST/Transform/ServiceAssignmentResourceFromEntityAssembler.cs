using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;
public static class ServiceAssignmentResourceFromEntityAssembler
{
    public static ServiceAssignmentResource ToResourceFromEntity(ServiceAssignment entity)
    {
        return new ServiceAssignmentResource(
            entity.AssignmentId.Value,
            entity.RequestId.Value,
            entity.TechnicianId?.Value ?? string.Empty,
            ToResourceFromSnapshot(entity.RecipeSnapshot),
            entity.Status.ToString(),
            entity.FailureReason,
            entity.RetryCount,
            entity.CreatedDate?.UtcDateTime ?? DateTime.MinValue,
            entity.MatchingScore != null ? MatchingScoreResourceAssembler.ToResource(entity.MatchingScore) : null
        );
    }
    private static RecipeSnapshotResource ToResourceFromSnapshot(RecipeSnapshot? snapshot)
    {
        if (snapshot is null) return null!;
        return new RecipeSnapshotResource(
            snapshot.RecipeId.Value,
            snapshot.ServiceName,
            snapshot.ServiceCategory.ToString(),
            snapshot.ComponentRequirements.Select(c => new ComponentRequirementResource(
                c.ComponentTypeId,
                c.Quantity,
                c.IsRequired
            )).ToList(),
            snapshot.Pricing.TotalPrice.Amount,
            snapshot.Pricing.TotalPrice.Currency.ToString(),
            snapshot.EstimatedDuration.TotalMinutes,
            snapshot.WarrantyPeriod.Months,
            snapshot.SnapshotAt
        );
    }
}
