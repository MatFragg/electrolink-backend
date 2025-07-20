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
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Operations related to Services")]
public class ServicesController : ControllerBase
{
    private readonly IServiceCommandService _cmd;
    private readonly IServiceQueryService _qry;

    public ServicesController(IServiceCommandService cmd, IServiceQueryService qry)
    {
        _cmd = cmd;
        _qry = qry;
    }

    [HttpGet("{serviceId}")]
    [SwaggerOperation(Summary = "Get Service by ID", OperationId = "GetServiceById")]
    [SwaggerResponse(200, "Service found", typeof(ServiceResource))]
    [SwaggerResponse(404, "Service not found")]
    public async Task<IActionResult> GetById(string serviceId)
    {
        var svc = await _qry.Handle(new GetServiceByIdQuery(serviceId));
        if (svc is null) return NotFound();
        var res = ServiceResourceFromEntityAssembler.ToResourceFromEntity(svc);
        return Ok(res);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new Service", OperationId = "CreateService")]
    [SwaggerResponse(201, "Service created", typeof(ServiceResource))]
    [SwaggerResponse(400, "Creation failed")]
    public async Task<IActionResult> Create([FromBody] CreateServiceResource resource)
    {
        var cmd = CreateServiceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var svc = await _cmd.Handle(cmd);
        if (svc is null) return BadRequest("Service creation failed");
        var res = ServiceResourceFromEntityAssembler.ToResourceFromEntity(svc);
        return CreatedAtAction(nameof(GetById), new { serviceId = res.ServiceId }, res);
    }
    
    [HttpPut("{serviceId}")]
    [SwaggerOperation(Summary = "Update a Service", OperationId = "UpdateService")]
    [SwaggerResponse(StatusCodes.Status200OK, "Service updated", typeof(ServiceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Service not found")]
    public async Task<IActionResult> UpdateService(string serviceId, [FromBody] CreateServiceResource resource)
    {
        var command = new UpdateServiceCommand(serviceId, resource.Name, resource.Description, resource.BasePrice,
            resource.EstimatedTime, resource.Category, resource.IsVisible, resource.CreatedBy);
        var updated = await _cmd.UpdateAsync(command);
        if (updated is null) return NotFound();
        return Ok(ServiceResourceFromEntityAssembler.ToResourceFromEntity(updated));
    }

    [HttpDelete("{serviceId}")]
    [SwaggerOperation(Summary = "Delete a Service", OperationId = "DeleteService")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Service deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Service not found")]
    public async Task<IActionResult> DeleteService(string serviceId)
    {
        var command = new DeleteServiceCommand(serviceId);
        var deleted = await _cmd.DeleteAsync(command);
        return deleted ? NoContent() : NotFound();
    }
}
