namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record UsageCounters
{
    public int MonthlyRequestsUsed  { get; }
    public int MonthlyRequestsLimit { get; }

    // 🔥 EF-friendly constructor
    public UsageCounters(int monthlyRequestsUsed, int monthlyRequestsLimit)
    {
        if (monthlyRequestsUsed < 0)
            throw new ArgumentException("MonthlyRequestsUsed cannot be negative.");
        if (monthlyRequestsLimit <= 0)
            throw new ArgumentException("MonthlyRequestsLimit must be positive.");

        MonthlyRequestsUsed  = monthlyRequestsUsed;
        MonthlyRequestsLimit = monthlyRequestsLimit;
    }

    public static UsageCounters Initial() => new(0, 2);

    public UsageCounters Increment()
    {
        if (MonthlyRequestsUsed >= MonthlyRequestsLimit)
            throw new InvalidOperationException("Monthly request limit already reached.");

        return new UsageCounters(MonthlyRequestsUsed + 1, MonthlyRequestsLimit);
    }

    public UsageCounters Reset() => new(0, MonthlyRequestsLimit);

    public bool HasCapacity => MonthlyRequestsUsed < MonthlyRequestsLimit;
    public int Remaining => MonthlyRequestsLimit - MonthlyRequestsUsed;
}
