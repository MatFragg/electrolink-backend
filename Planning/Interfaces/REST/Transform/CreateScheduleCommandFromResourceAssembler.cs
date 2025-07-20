using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Transform;

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