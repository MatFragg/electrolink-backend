namespace Hampcoders.Electrolink.API.Analytics.Interfaces.REST.Resources;

public record TimeSeriesResource(
    DateTime Timestamp,
    decimal KilowattHours,
    string Granularity);
