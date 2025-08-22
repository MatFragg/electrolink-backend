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
    public async Task<Subscription?> FindBySubscriptionIdAsync(SubscriptionId id)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Subscription?> FindByUserIdAsync(UserId userId)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }
}