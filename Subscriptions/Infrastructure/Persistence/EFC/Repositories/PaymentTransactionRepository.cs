using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Entity Framework Core implementation of <see cref="IPaymentTransactionRepository"/>.
/// </summary>
public class PaymentTransactionRepository(AppDbContext context) : BaseRepository<PaymentTransaction>(context), IPaymentTransactionRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<PaymentTransaction>> ListBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await Context.Set<PaymentTransaction>()
            .Where(pt => pt.SubscriptionId.Value == subscriptionId)
            .ToListAsync();
    }

}
