namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;

public record CreateScheduleCommand(
    string ScheduleId,
    Guid TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);