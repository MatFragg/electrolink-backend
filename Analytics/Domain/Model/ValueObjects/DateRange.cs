namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    private DateRange(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new ArgumentException("End date must be after start date.");
        Start = start;
        End = end;
    }

    public static DateRange Of(DateTime start, DateTime end) => new(start, end);
    public static DateRange CurrentMonth()
    {
        var now = DateTime.UtcNow;
        var start = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = start.AddMonths(1).AddTicks(-1);
        return new DateRange(start, end);
    }
}
