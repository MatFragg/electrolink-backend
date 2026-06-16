namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

public record PaymentHistoryResource(
    int Page,
    int PageSize,
    IEnumerable<PaymentRecordResource> Payments);

