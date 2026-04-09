namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

public record NoShowAlertResource(
    string ExecutionId,
    DateTime ScheduledAt,
    int MinutesLate,
    string TechnicianFullName,
    string TechnicianPhone,
    IReadOnlyList<string> Options
);