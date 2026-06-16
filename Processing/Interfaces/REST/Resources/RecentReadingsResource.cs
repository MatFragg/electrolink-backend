namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;

public record RecentReadingsResource(
    string   DeviceId,
    string   PropertyId,
    int      WindowMinutes,
    IReadOnlyList<ReadingValueResource> Readings,
    float    AverageConsumptionKw,
    float    PeakCurrentLast60Min
);
