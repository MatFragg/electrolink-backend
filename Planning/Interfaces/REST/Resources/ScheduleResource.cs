namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Resources;

public record ScheduleResource(
    string ScheduleId,
    string TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);