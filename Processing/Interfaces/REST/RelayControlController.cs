using System.Security.Claims;
using Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Processing.Domain.Services;
using Hampcoders.Electrolink.API.Processing.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Processing.Interfaces.REST;

[ApiController]
[Route("api/v1/iot/devices/{deviceId}/relay")]
[Produces("application/json")]
public class RelayControlController(IRelayCommandService relayService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Issue relay command (technician)", OperationId = "IssueRelayCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> IssueRelayCommand(
        string deviceId,
        [FromBody] IssueRelayCommandResource resource)
    {
        try
        {
            var technicianId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? throw new UnauthorizedAccessException("Technician identity not found.");

            var command = new IssueRelayCommandCommand(
                deviceId, resource.PropertyId, technicianId,
                resource.ServiceRequestId, resource.TargetRelayState);

            await relayService.Handle(command);
            return NoContent();
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException ex)   { return BadRequest(new { message = ex.Message }); }
        catch (KeyNotFoundException ex)        { return NotFound(new { message = ex.Message }); }
    }
}
