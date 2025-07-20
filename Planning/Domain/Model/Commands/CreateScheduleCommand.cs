namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;

public record CreateScheduleCommand(
    string ScheduleId,
    Guid TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);