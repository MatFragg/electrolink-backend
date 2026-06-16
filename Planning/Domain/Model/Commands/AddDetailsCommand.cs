using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record AddDetailsCommand(
    RequestId RequestId,
    string ProblemDescription,
    decimal ConsumptionKwh,
    decimal AmountPaid,
    string BillingPeriod,
    string ReceiptNumber,
    IReadOnlyList<DateOnly> PreferredDates,
    ETimePreference TimePreference,
    bool IsPriority
);
