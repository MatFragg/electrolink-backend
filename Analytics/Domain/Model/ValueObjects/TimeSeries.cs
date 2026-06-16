using Hampcoders.Electrolink.API.Analytics.Domain.Model.Enums;

namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record TimeSeries
{
    public DateTime Timestamp { get; }
    public decimal KilowattHours { get; }
    public Granularity Granularity { get; }

    private TimeSeries(DateTime timestamp, decimal kilowattHours, Granularity granularity)
    {
        if (kilowattHours < 0) throw new ArgumentException("KilowattHours cannot be negative.");
        Timestamp = timestamp;
        KilowattHours = kilowattHours;
        Granularity = granularity;
    }

    public static TimeSeries Of(DateTime timestamp, decimal kilowattHours, Granularity granularity) =>
        new(timestamp, kilowattHours, granularity);
}
