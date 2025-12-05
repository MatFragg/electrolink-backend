namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record ScheduleResource(
    string ScheduleId,
    string TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);