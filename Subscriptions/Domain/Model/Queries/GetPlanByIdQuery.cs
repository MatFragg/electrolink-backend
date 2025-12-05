namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

/// <summary>
/// Query to get a plan by its ID.
/// </summary>
/// <param name="PlanId">The ID of the plan.</param>
public record GetPlanByIdQuery(Guid PlanId);