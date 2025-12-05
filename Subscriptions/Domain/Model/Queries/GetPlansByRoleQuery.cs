using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

/// <summary>
/// Query to get all plans for a specific user role.
/// </summary>
/// <param name="Role">The target user role.</param>
public record GetPlansByRoleQuery(EUserRole Role);