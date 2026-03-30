using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record AddDetailsCommand(
    string RequestId,
    string ProblemDescription,
    decimal ConsumptionKwh,
    decimal AmountPaid,
    string BillingPeriod,
    string ReceiptNumber,
    IReadOnlyList<DateOnly> PreferredDates,
    string TimePreference,
    bool IsPriority
);

