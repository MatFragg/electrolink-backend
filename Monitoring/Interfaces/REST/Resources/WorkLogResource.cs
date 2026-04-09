namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record WorkLogResource(
    string ExecutionId,
    IReadOnlyList<PhotoResource> Photos,
    string? ReportContent,
    string? Findings,
    string? Recommendations,
    int ReportVersion,
    IReadOnlyList<ComponentUsageResource> Components
);