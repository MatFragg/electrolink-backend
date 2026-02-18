using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class CreateScheduleCommandFromResourceAssembler
{
    public static CreateScheduleCommand ToCommandFromResource(CreateScheduleResource r) =>
        new CreateScheduleCommand(
            Guid.NewGuid().ToString(),
            r.TechnicianId,
            r.Day,
            r.StartTime,
            r.EndTime
        );
}