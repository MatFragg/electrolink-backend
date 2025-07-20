namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

public record ElectricBill(
    string BillingPeriod,
    double EnergyConsumed,
    double AmountPaid,
    string BillImageUrl
);