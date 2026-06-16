using Microsoft.AspNetCore.Authorization;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Authorization;

public class SubscriptionTierRequirement : IAuthorizationRequirement
{
    public string[] AllowedTiers { get; }

    public SubscriptionTierRequirement(params string[] allowedTiers)
    {
        AllowedTiers = allowedTiers;
    }
}
