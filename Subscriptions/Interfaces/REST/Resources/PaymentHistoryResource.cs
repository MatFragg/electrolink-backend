namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record PaymentHistoryResource(IEnumerable<PaymentRecordResource> Payments);

