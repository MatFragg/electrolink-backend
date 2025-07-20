namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Resources;

public record CreateScheduleResource(
    Guid TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);