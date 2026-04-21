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
    /// <inheritdoc/>
    public async Task<Subscription?> FindByUserIdAsync(UserId userId)
        => await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.UserId == userId);
    
    /// <inheritdoc/>
    public async Task<IEnumerable<Subscription>> ListActiveAsync()
        => await Context.Set<Subscription>()
            .Where(s => s.Status == ESubscriptionStatus.Active || s.Status == ESubscriptionStatus.Trial)
            .ToListAsync();
    
    /// <inheritdoc/>
    public async Task<Subscription?> FindByPaymentGatewayCustomerIdAsync(PaymentGatewayCustomerId gatewayCustomerId)
        => await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.GatewayCustomerId == gatewayCustomerId);
    

    /// <inheritdoc/>
    public async Task<Subscription?> FindByPaymentGatewaySubscriptionIdAsync(PaymentGatewaySubscriptionId gatewaySubscriptionId)
        => await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.GatewaySubscriptionId == gatewaySubscriptionId);
    
    
    public async Task<Subscription?> FindActiveByUserIdAsync(UserId userId)
        => await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.UserId == userId && (s.Status == ESubscriptionStatus.Active || s.Status == ESubscriptionStatus.Trial));

    public async Task<IEnumerable<Subscription>> FindAllInGracePeriodExpiredAsync(DateTime asOf)
        => await Context.Set<Subscription>()
            .Where(s => s.Status == ESubscriptionStatus.GracePeriod && s.GracePeriodEndsAt != null && s.GracePeriodEndsAt <= asOf)
            .ToListAsync();

    public async Task<IEnumerable<Subscription>> FindAllBasicHomeownersAsync()
        => await Context.Set<Subscription>()
            .Where(s => s.PlanType.Value == EPlanType.Basic && s.BusinessRole.Value == EBusinessRole.Homeowner)
            .ToListAsync();
}