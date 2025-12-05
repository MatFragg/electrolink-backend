using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command to delete a plan.
/// </summary>
/// <param name="PlanId">The ID of the plan to delete.</param>
public record DeletePlanCommand(Guid PlanId);