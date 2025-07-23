using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Transform;

public static class ScheduleResourceFromEntityAssembler
{
    public static ScheduleResource ToResourceFromEntity(Schedule s) =>
        new ScheduleResource(
            s.ScheduleId,
            s.TechnicianId.ToString(),
            s.Day,
            s.StartTime,
            s.EndTime
        );
}