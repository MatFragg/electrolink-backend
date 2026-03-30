using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ServiceRequestSummary(RequestId RequestId, HomeownerId HomeownerId, PropertyId PropertyId, RecipeId SelectedRecipeId, TechnicianId SelectedTechnicianId, ERequestStatus Status, bool IsPriority, DateTime CreatedAt);