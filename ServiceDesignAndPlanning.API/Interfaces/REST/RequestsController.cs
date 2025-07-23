using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;
using Swashbuckle.AspNetCore.Annotations;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Queries;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Transform;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST;

[ApiController]
[Route("api/v1/")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Operations related to Service Requests")]
public class RequestsController : ControllerBase
{
    private readonly IRequestCommandService _cmd;
    private readonly IRequestQueryService _qry;

    public RequestsController(IRequestCommandService cmd, IRequestQueryService qry)
    {
        _cmd = cmd;
        _qry = qry;
    }

    [HttpGet("[controller]/{requestId}")]
    [SwaggerOperation(Summary = "Get Request by ID", OperationId = "GetRequestById")]
    [SwaggerResponse(200, "Request found", typeof(RequestResource))]
    [SwaggerResponse(404, "Request not found")]
    public async Task<IActionResult> GetById(string requestId)
    {
        var req = await _qry.Handle(new GetRequestDetailsQuery(requestId));
        if (req is null) return NotFound();
        var res = RequestResourceFromEntityAssembler.ToResourceFromEntity(req);
        return Ok(res);
    }

    [HttpGet("clients/{clientId}/[controller]")]
    [SwaggerOperation(Summary = "Get Requests by Client", OperationId = "GetRequestsByClient")]
    [SwaggerResponse(200, "Requests found", typeof(IEnumerable<RequestResource>))]
    [SwaggerResponse(400, "Invalid client ID format")]
    public async Task<IActionResult> GetByClient(string clientId)
    {
        if (!Guid.TryParse(clientId, out var parsedClientId))
            return BadRequest("Invalid client ID format.");

        var reqs = await _qry.Handle(new GetRequestsByClientIdQuery(parsedClientId));
        var resources = reqs.Select(RequestResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }


    [HttpPost("[controller]")]
    [SwaggerOperation(Summary = "Create a new Request", OperationId = "CreateRequest")]
    [SwaggerResponse(201, "Request created", typeof(RequestResource))]
    [SwaggerResponse(400, "Creation failed")]
    public async Task<IActionResult> Create([FromBody] CreateRequestResource resource)
    {
        var cmd = CreateRequestCommandFromResourceAssembler.ToCommandFromResource(resource);
        var req = await _cmd.Handle(cmd);
        if (req is null) return BadRequest("Request creation failed");
        var res = RequestResourceFromEntityAssembler.ToResourceFromEntity(req);
        return CreatedAtAction(nameof(GetById), new { requestId = res.RequestId }, res);
    }
    
    [HttpPut("[controller]/{requestId}")]
    [SwaggerOperation(Summary = "Update a Request", OperationId = "UpdateRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request updated", typeof(RequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Request not found")]
    public async Task<IActionResult> UpdateRequest(string requestId, [FromBody] CreateRequestResource resource)
    {
        var command = new UpdateRequestCommand(
            requestId,
            resource.ClientId,
            resource.TechnicianId,
            resource.PropertyId,
            resource.ServiceId,
            resource.ScheduledDate,
            resource.ProblemDescription,
            resource.Bill,
            resource.Photos
        );

        var updated = await _cmd.UpdateAsync(command);
        if (updated is null) return NotFound();
        return Ok(RequestResourceFromEntityAssembler.ToResourceFromEntity(updated));
    }
    [HttpDelete("[controller]/{requestId}")]
    [SwaggerOperation(Summary = "Delete a Request", OperationId = "DeleteRequest")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Request deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Request not found")]
    public async Task<IActionResult> DeleteRequest(string requestId)
    {
        var deleted = await _cmd.DeleteAsync(new DeleteRequestCommand(requestId));
        return deleted ? NoContent() : NotFound();
    }
}
