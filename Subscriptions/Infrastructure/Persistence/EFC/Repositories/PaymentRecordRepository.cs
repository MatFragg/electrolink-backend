using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class PaymentRecordRepository(AppDbContext context)
    : BaseRepository<PaymentRecord, PaymentRecordId>(context), IPaymentRecordRepository
{
    public async Task<bool> ExistsByStripeInvoiceIdAsync(string stripeInvoiceId)
        => await Context.Set<PaymentRecord>()
            .AnyAsync(p => p.StripeInvoiceId.Value == stripeInvoiceId);

    public async Task<IEnumerable<PaymentRecord>> FindBySubscriptionIdAsync(string subscriptionId)
        => await Context.Set<PaymentRecord>()
            .Where(p => p.SubscriptionId.Value == subscriptionId)
            .OrderByDescending(p => p.ProcessedAt)
            .AsNoTracking()
            .ToListAsync();
}
