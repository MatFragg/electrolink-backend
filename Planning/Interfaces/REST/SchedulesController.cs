using System.Net.Mime;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.API.Domain.Services;
using Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.REST;

[ApiController]
[Route("api/v1/")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Operations related to Technician Schedules")]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleCommandService _cmd;
    private readonly IScheduleQueryService _qry;

    public SchedulesController(
        IScheduleCommandService cmd,
        IScheduleQueryService qry
    )
    {
        _cmd = cmd;
        _qry = qry;
    }

    [HttpGet("technicians/{technicianId}/[controller]")]
    [SwaggerOperation(Summary = "Get Schedule by Technician", OperationId = "GetScheduleByTechnician")]
    [SwaggerResponse(200, "Schedules found", typeof(IEnumerable<ScheduleResource>))]
    public async Task<IActionResult> GetByTechnician(string technicianId)
    {
        var guid = Guid.Parse(technicianId); // ✅ Conversión
        var scheds = await _qry.Handle(new GetScheduleByTechnicianIdQuery(guid)); // ✅ Usar guid
        var resources = scheds.Select(ScheduleResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }


    [HttpPost("[controller]")]
    [SwaggerOperation(Summary = "Create a new Schedule", OperationId = "CreateSchedule")]
    [SwaggerResponse(201, "Schedule created", typeof(ScheduleResource))]
    [SwaggerResponse(400, "Creation failed")]
    public async Task<IActionResult> Create([FromBody] CreateScheduleResource resource)
    {
        var cmd = CreateScheduleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var sched = await _cmd.CreateAsync(cmd); // if you renamed accordingly
        if (sched is null) return BadRequest("Schedule creation failed");
        var res = ScheduleResourceFromEntityAssembler.ToResourceFromEntity(sched);
        return CreatedAtAction(nameof(GetByTechnician), new { technicianId = res.TechnicianId }, res);
    }
    [HttpPut("[controller]/{scheduleId}")]
    public async Task<IActionResult> UpdateSchedule(string scheduleId, [FromBody] CreateScheduleResource resource)
    {
        var command = new UpdateScheduleCommand(
            scheduleId,
            resource.Day,
            resource.StartTime,
            resource.EndTime
        );

        var updated = await _cmd.UpdateAsync(command);
        if (updated is null) return NotFound();
        return Ok(ScheduleResourceFromEntityAssembler.ToResourceFromEntity(updated));
    }

    [HttpDelete("[controller]/{scheduleId}")]
    public async Task<IActionResult> DeleteSchedule(string scheduleId)
    {
        var command = new DeleteScheduleCommand(scheduleId);
        var deleted = await _cmd.DeleteAsync(command);
        return deleted ? NoContent() : NotFound();
    }
}
