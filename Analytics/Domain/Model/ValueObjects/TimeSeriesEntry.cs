namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record TimeSeriesEntry(DateTime Timestamp, decimal KilowattHours, string Granularity);
