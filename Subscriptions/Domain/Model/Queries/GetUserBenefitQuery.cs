namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

/// <summary>
/// Query to check a user's plan benefits for a specific functionality.
/// </summary>
/// <param name="UserId">The ID of the user.</param>
/// <param name="BenefitType">The type of benefit to check (e.g., "MaxServiceRequests").</param>
public record GetUserBenefitQuery(int UserId, string BenefitType);