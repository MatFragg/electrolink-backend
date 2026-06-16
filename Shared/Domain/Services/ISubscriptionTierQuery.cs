namespace Hampcoders.Electrolink.API.Shared.Domain.Services;

public interface ISubscriptionTierQuery
{
    Task<string?> GetTierAsync(string userId);
}
