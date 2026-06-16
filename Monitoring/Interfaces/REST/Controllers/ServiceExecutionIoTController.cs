using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Interfaces.REST;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/service-executions/{executionId}/iot")]
[Produces("application/json")]
public class ServiceExecutionIoTController(
    IServiceExecutionCommandService commandService) : ControllerBase
{
    [HttpPost("toggle-relay")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceExecutionResource>> ToggleRelay(
        [FromBody] ToggleRelayResource resource)
    {
        try
        {
            var executionId = RouteData.Values["executionId"]?.ToString()
                ?? throw new ArgumentException("executionId is required in the route.");
            var technicianId = User.GetTechnicianId();
            var targetState = Enum.Parse<ERelayState>(resource.TargetState, true);
            var command = new RemotelyToggleCircuitCommand(
                ServiceExecutionId.From(executionId),
                TechnicianId.From(technicianId),
                targetState,
                technicianId);

            var execution = await commandService.Handle(command);
            return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (KeyNotFoundException ex)      { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        catch (UnauthorizedAccessException)  { return Forbid(); }
    }
}

public record ToggleRelayResource(string TargetState);
