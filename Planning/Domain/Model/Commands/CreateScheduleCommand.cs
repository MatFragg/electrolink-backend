namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record CreateScheduleCommand(
    string ScheduleId,
    Guid TechnicianId,
    string Day,
    string StartTime,
    string EndTime
);