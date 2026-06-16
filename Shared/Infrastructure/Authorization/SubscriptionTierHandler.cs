using System.Security.Claims;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Authorization;

public class SubscriptionTierHandler : AuthorizationHandler<SubscriptionTierRequirement>
{
    private readonly ISubscriptionTierQuery _query;

    public SubscriptionTierHandler(ISubscriptionTierQuery query)
    {
        _query = query;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SubscriptionTierRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return;

        var tier = await _query.GetTierAsync(userId);
        if (tier is not null && requirement.AllowedTiers.Contains(tier))
            context.Succeed(requirement);
    }
}
