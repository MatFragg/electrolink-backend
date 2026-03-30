using System.Text.Json.Serialization;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record RequestPreferences
{
    public string ProblemDescription { get; init; }
    public decimal ConsumptionKwh { get; init; }
    public Money AmountPaid { get; init; }
    public string BillingPeriod { get; init; }
    public string ReceiptNumber { get; init; }
    public IReadOnlyList<DateOnly> PreferredDates { get; init; }
    public ETimePreference TimePreference { get; init; }

    private RequestPreferences() { }

    [JsonConstructor]
    public RequestPreferences(
        string problemDescription,
        decimal consumptionKwh,
        Money amountPaid,
        string billingPeriod,
        string receiptNumber,
        IReadOnlyList<DateOnly> preferredDates,
        ETimePreference timePreference)
    {
        ProblemDescription = problemDescription;
        ConsumptionKwh     = consumptionKwh;
        AmountPaid         = amountPaid;
        BillingPeriod      = billingPeriod;
        ReceiptNumber      = receiptNumber;
        PreferredDates     = preferredDates;
        TimePreference     = timePreference;
    }

    public static RequestPreferences Create(
        string problemDescription,
        decimal consumptionKwh,
        Money amountPaid,
        string billingPeriod,
        string receiptNumber,
        IReadOnlyList<DateOnly> preferredDates,
        ETimePreference timePreference)
    {
        if (preferredDates.Any(d => d < DateOnly.FromDateTime(DateTime.Today.AddDays(2))))
            throw new InvalidPreferredDateException();

        return new RequestPreferences(
            problemDescription, 
            consumptionKwh, 
            amountPaid,
            billingPeriod, 
            receiptNumber, 
            preferredDates, 
            timePreference);
    }
}
