using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record ReceiptDataDto(
    decimal ConsumptionKwh,
    decimal AmountPaid,
    string Currency,
    string BillingPeriod,
    string ReceiptNumber
);

