namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record WorkLogSummaryResource(
    int PhotosCount,
    bool HasReport,
    bool HasComponents
);