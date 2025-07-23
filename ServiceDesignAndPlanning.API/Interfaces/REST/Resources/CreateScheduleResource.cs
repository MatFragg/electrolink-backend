namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

public record CreateScheduleResource(
    Guid TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);