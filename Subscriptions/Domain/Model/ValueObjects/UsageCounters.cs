namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record UsageCounters
{
    public int MonthlyRequestsUsed { get; }
    public int MonthlyRequestsLimit { get; }

    private UsageCounters(int monthlyRequestsUsed, int monthlyRequestsLimit)
    {
        MonthlyRequestsUsed = monthlyRequestsUsed;
        MonthlyRequestsLimit = monthlyRequestsLimit;
    }

    public bool HasCapacity => MonthlyRequestsUsed < MonthlyRequestsLimit;

    public int Remaining => Math.Max(MonthlyRequestsLimit - MonthlyRequestsUsed, 0);

    public static UsageCounters Initial(int monthlyLimit = 2)
    {
        if (monthlyLimit <= 0)
            throw new ArgumentException("Monthly limit must be greater than zero.");

        return new UsageCounters(0, monthlyLimit);
    }

    public UsageCounters Increment()
    {
        return new UsageCounters(MonthlyRequestsUsed + 1, MonthlyRequestsLimit);
    }

    public UsageCounters Reset()
    {
        return new UsageCounters(0, MonthlyRequestsLimit);
    }
}

