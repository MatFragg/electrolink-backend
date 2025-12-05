using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    
    /// <inheritdoc/>
    public async Task<Subscription?> FindBySubscriptionIdAsync(SubscriptionId id)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Subscription?> FindByUserIdAsync(UserId userId)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<Subscription>> ListActiveAsync()
    {
        return await Context.Set<Subscription>()
            .Where(s => s.Status == ESubscriptionStatus.Active || s.Status == ESubscriptionStatus.Trial)
            .ToListAsync();
    }
    
    /// <inheritdoc/>
    public async Task<Subscription?> FindByStripeCustomerIdAsync(string stripeCustomerId)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.StripeCustomerId == stripeCustomerId);
    }

    /// <inheritdoc/>
    public async Task<Subscription?> FindByStripeSubscriptionIdAsync(string stripeSubscriptionId)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.StripeSubscriptionId == stripeSubscriptionId);
    }
}