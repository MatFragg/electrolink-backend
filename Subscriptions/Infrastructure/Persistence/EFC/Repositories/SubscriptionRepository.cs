using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription, SubscriptionId>(context), ISubscriptionRepository
{
    public async Task<Subscription?> FindByUserIdAsync(string userId)
        => await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.UserId.Value == userId);

    public async Task<Subscription> FindByUserIdOrFailAsync(string userId)
        => await FindByUserIdAsync(userId)
           ?? throw new ArgumentException($"No subscription found for user {userId}.");

    public async Task<Subscription?> FindByStripeCustomerIdAsync(string stripeCustomerId)
        => await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.StripeCustomerId.Value == stripeCustomerId);

    public async Task<Subscription> FindByStripeCustomerIdOrFailAsync(string stripeCustomerId)
        => await FindByStripeCustomerIdAsync(stripeCustomerId)
           ?? throw new ArgumentException($"No subscription found for Stripe customer {stripeCustomerId}.");

    public async Task<Subscription?> FindByStripeSubscriptionIdAsync(string stripeSubscriptionId)
        => await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.StripeSubscriptionId != null && s.StripeSubscriptionId.Value == stripeSubscriptionId);

    public async Task<Subscription> FindByStripeSubscriptionIdOrFailAsync(string stripeSubscriptionId)
        => await FindByStripeSubscriptionIdAsync(stripeSubscriptionId)
           ?? throw new ArgumentException($"No subscription found for Stripe subscription {stripeSubscriptionId}.");

    public async Task<bool> ExistsByUserIdAsync(UserId userId)
        => await Context.Set<Subscription>().AnyAsync(s => s.UserId == userId);

    public async Task<IEnumerable<Subscription>> FindAllInGracePeriodExpiredAsync(DateTime asOf)
        => await Context.Set<Subscription>()
            .Where(s => s.Status.Value == ESubscriptionStatus.GracePeriod && s.GracePeriodEndsAt != null && s.GracePeriodEndsAt <= asOf)
            .ToListAsync();

    public async Task<IEnumerable<Subscription>> FindAllBasicHomeownersAsync()
        => await Context.Set<Subscription>()
            .Where(s => s.PlanType.Value == EPlanType.Basic && s.BusinessRole.Value == EBusinessRole.Homeowner)
            .ToListAsync();
}