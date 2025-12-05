namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

public record CreateScheduleResource(
    Guid TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);