using System.Net.Mime;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST;

[ApiController]
[Route("api/v1/planning/assignments")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Service Assignment Query Endpoints")]
public class ServiceAssignmentController(
    IServiceAssignmentQueryService assignmentQueryService,
    ILogger<ServiceAssignmentController> logger)
    : ControllerBase
{
    /// <summary>
    /// Get a specific service assignment by ID
    /// </summary>
    [HttpGet("{serviceId:guid}")]
    [SwaggerOperation(
        Summary = "Get Assignment by ID",
        Description = "Retrieves a specific service assignment by its service ID.",
        OperationId = "GetAssignmentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Assignment found", typeof(ServiceAssignmentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Assignment not found")]
    public async Task<IActionResult> GetAssignmentById(Guid serviceId)
    {
        var query = new GetServiceAssignmentByIdQuery(serviceId);
        var assignment = await assignmentQueryService.Handle(query);

        if (assignment is null)
            return NotFound(new { message = $"Assignment {serviceId} not found" });

        var resource = ServiceAssignmentResourceFromEntityAssembler.ToResourceFromEntity(assignment);
        return Ok(resource);
    }

    /// <summary>
    /// Get all assignments for a technician
    /// </summary>
    [HttpGet("technician/{technicianId:guid}")]
    [SwaggerOperation(
        Summary = "Get Assignments by Technician",
        Description = "Retrieves all service assignments for a specific technician.",
        OperationId = "GetAssignmentsByTechnician")]
    [SwaggerResponse(StatusCodes.Status200OK, "Assignments retrieved", typeof(List<ServiceAssignmentListResource>))]
    public async Task<IActionResult> GetAssignmentsByTechnician(Guid technicianId)
    {
        var query = new GetAssignmentsByTechnicianQuery(technicianId);
        var assignments = await assignmentQueryService.Handle(query);

        var resources = assignments.Select(
            ServiceAssignmentResourceFromEntityAssembler.ToListResourceFromEntity);
        
        logger.LogInformation("Retrieved {Count} assignments for technician {TechnicianId}",
            assignments.Count(), technicianId);

        return Ok(resources);
    }

    /// <summary>
    /// Get all assignments for a homeowner
    /// </summary>
    [HttpGet("homeowner/{homeownerId:guid}")]
    [SwaggerOperation(
        Summary = "Get Assignments by Homeowner",
        Description = "Retrieves all service assignments for a specific homeowner.",
        OperationId = "GetAssignmentsByHomeowner")]
    [SwaggerResponse(StatusCodes.Status200OK, "Assignments retrieved", typeof(List<ServiceAssignmentListResource>))]
    public async Task<IActionResult> GetAssignmentsByHomeowner(Guid homeownerId)
    {
        var query = new GetAssignmentsByHomeownerQuery(homeownerId);
        var assignments = (await assignmentQueryService.Handle(query)).ToList();

        var resources = assignments.Select(
            ServiceAssignmentResourceFromEntityAssembler.ToListResourceFromEntity);

        logger.LogInformation("Retrieved {Count} assignments for homeowner {HomeownerId}",
            assignments.Count, homeownerId);

        return Ok(resources);
    }

    /// <summary>
    /// Get assignment by request ID
    /// </summary>
    [HttpGet("request/{requestId:guid}")]
    [SwaggerOperation(
        Summary = "Get Assignment by Request ID",
        Description = "Retrieves the service assignment associated with a specific request.",
        OperationId = "GetAssignmentByRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "Assignment found", typeof(ServiceAssignmentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No assignment found for this request")]
    public async Task<IActionResult> GetAssignmentByRequest(Guid requestId)
    {
        var query = new GetAssignmentByRequestIdQuery(requestId);
        var assignment = await assignmentQueryService.Handle(query);

        if (assignment is null)
            return NotFound(new { message = $"No assignment found for request {requestId}" });

        var resource = ServiceAssignmentResourceFromEntityAssembler.ToResourceFromEntity(assignment);
        return Ok(resource);
    }
}


