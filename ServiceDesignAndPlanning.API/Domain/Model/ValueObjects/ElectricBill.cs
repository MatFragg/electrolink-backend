namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;

public record ElectricBill(
    string BillingPeriod,
    double EnergyConsumed,
    double AmountPaid,
    string BillImageUrl
);