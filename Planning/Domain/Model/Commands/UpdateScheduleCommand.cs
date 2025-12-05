namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

public record UpdateScheduleCommand(
    string ScheduleId,
    string Day,
    string StartTime,
    string EndTime
);
