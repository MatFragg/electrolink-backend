namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record BillingPeriod
{
    public DateTime PeriodStart { get; }
    public DateTime PeriodEnd { get; }

    private BillingPeriod(DateTime periodStart, DateTime periodEnd)
    {
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    public static BillingPeriod Of(DateTime periodStart, DateTime periodEnd)
    {
        if (periodEnd <= periodStart)
            throw new ArgumentException("BillingPeriod end must be greater than start.");

        return new BillingPeriod(periodStart, periodEnd);
    }

    public bool IsActive(DateTime at) => at >= PeriodStart && at <= PeriodEnd;
}
