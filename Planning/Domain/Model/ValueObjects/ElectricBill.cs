namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ElectricBill(
    string BillingPeriod,
    double EnergyConsumed,
    double AmountPaid,
    string BillImageUrl
);