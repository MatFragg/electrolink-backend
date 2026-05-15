using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class PaymentHistoryResourceFromEntityAssembler
{
    public static PaymentHistoryResource ToResource(IEnumerable<PaymentRecord> records)
        => new(records.Select(r => new PaymentRecordResource(
            PaymentRecordId: r.PaymentRecordId.Value,
            StripeInvoiceId: r.StripeInvoiceId.Value,
            AmountDecimal: r.Amount.Amount / 100.0m,
            Currency: r.Amount.Currency,
            Status: r.Status.ToString(),
            ProcessedAt: r.ProcessedAt.ToString("O"))));
}

