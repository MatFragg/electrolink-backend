using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;

public interface IPaymentRecordRepository : IBaseRepository<PaymentRecord, PaymentRecordId>
{
    Task<bool> ExistsByStripeInvoiceIdAsync(StripeInvoiceId stripeInvoiceId);
    Task<IEnumerable<PaymentRecord>> FindBySubscriptionIdAsync(SubscriptionId subscriptionId);
}

