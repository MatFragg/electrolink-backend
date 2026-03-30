namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record AddServiceDetailsResource(
    string ProblemDescription,
    decimal ConsumptionKwh,
    decimal AmountPaid,
    string AmountCurrency,
    string BillingPeriod,
    string ReceiptNumber,
    bool IsPriority,
    IReadOnlyList<string> PreferredDates,
    string TimePreference);

