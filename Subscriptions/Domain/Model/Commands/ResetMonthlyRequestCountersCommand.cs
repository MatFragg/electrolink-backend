namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

/// <summary>
/// Command: Reset monthly request counters for all BASIC HOMEOWNER subscriptions.
/// Executed by MonthlyCounterResetJob on day 1 of each month (00:00 UTC).
/// </summary>
public record ResetMonthlyRequestCountersCommand(
    DateTime ResetDate);
