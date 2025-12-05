using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

/// <summary>
/// Query to get the default plan for a specific user role.
/// </summary>
/// <param name="Role">The target user role.</param>
public record GetDefaultPlanByRoleQuery(EUserRole Role);