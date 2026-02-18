using System.Net.Mime;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST;

[ApiController]
[Route("api/v1/planning/requests")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Service Request Management - Multi-Step Wizard Endpoints")]
public class ServiceRequestController(
    IServiceRequestCommandService requestCommandService,
    IServiceRequestQueryService requestQueryService,
    ILogger<ServiceRequestController> logger)
    : ControllerBase
{
    #region Wizard Steps

    /// <summary>
    /// Step 1: Initiate a new service request
    /// </summary>
    [HttpPost("initiate")]
    [SwaggerOperation(
        Summary = "Step 1 - Initiate Request",
        Description = "Initiates a new service request wizard. Creates a request in Draft status.",
        OperationId = "InitiateRequest")]
    [SwaggerResponse(StatusCodes.Status201Created, "Request initiated", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Cannot create request (e.g., monthly limit reached)")]
    public async Task<IActionResult> InitiateRequest([FromBody] InitiateServiceRequestResource resource)
    {
        try
        {
            var command = new InitiateServiceRequestCommand(resource.HomeownerId);
            var request = await requestCommandService.Handle(command);

            if (request is null)
                return BadRequest("Could not initiate service request.");

            var requestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(request);
            logger.LogInformation("Service request {RequestId} initiated for homeowner {HomeownerId}",
                request.Id.Id, resource.HomeownerId);

            return CreatedAtAction(nameof(GetRequestById),
                new { requestId = request.Id.Id, homeownerId = resource.HomeownerId }, requestResource);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Cannot initiate request");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error initiating request");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Step 2: Select property for the request
    /// </summary>
    [HttpPost("{requestId:guid}/select-property")]
    [SwaggerOperation(
        Summary = "Step 2 - Select Property",
        Description = "Associates a property with the service request.",
        OperationId = "SelectProperty")]
    [SwaggerResponse(StatusCodes.Status200OK, "Property selected", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Request not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid state transition")]
    public async Task<IActionResult> SelectProperty(
        Guid requestId,
        [FromQuery] Guid homeownerId,
        [FromBody] SelectPropertyResource resource)
    {
        try
        {
            var command = new SelectPropertyForRequestCommand(requestId, homeownerId, resource.PropertyId);
            var request = await requestCommandService.Handle(command);

            if (request is null)
                return NotFound(new { message = $"Request {requestId} not found" });

            var requestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(request);
            logger.LogInformation("Property {PropertyId} selected for request {RequestId}",
                resource.PropertyId, requestId);

            return Ok(requestResource);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error selecting property");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Step 3-4: Add service details (recipe, technician, receipt, preferences)
    /// </summary>
    [HttpPost("{requestId:guid}/add-details")]
    [SwaggerOperation(
        Summary = "Step 3-4 - Add Service Details",
        Description = "Adds service selection, receipt data, and preferences to the request.",
        OperationId = "AddServiceDetails")]
    [SwaggerResponse(StatusCodes.Status200OK, "Details added", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Request not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid state or priority requires Premium plan")]
    public async Task<IActionResult> AddServiceDetails(
        Guid requestId,
        [FromQuery] Guid homeownerId,
        [FromBody] AddServiceDetailsResource resource)
    {
        try
        {
            var command = ServiceRequestCommandFromResourceAssemblers.ToCommandFromResource(
                resource, requestId, homeownerId);
            var request = await requestCommandService.Handle(command);

            if (request is null)
                return NotFound(new { message = $"Request {requestId} not found" });

            var requestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(request);
            logger.LogInformation("Service details added for request {RequestId}", requestId);

            return Ok(requestResource);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding service details");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Step 5: Confirm the service request (triggers auto-assignment)
    /// </summary>
    [HttpPost("{requestId:guid}/confirm")]
    [SwaggerOperation(
        Summary = "Step 5 - Confirm Request",
        Description = "Confirms the service request, triggering automatic assignment to the selected technician.",
        OperationId = "ConfirmRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request confirmed and assignment initiated", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Request not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid state or missing required data")]
    public async Task<IActionResult> ConfirmRequest(
        Guid requestId,
        [FromQuery] Guid homeownerId)
    {
        try
        {
            var command = new ConfirmServiceRequestCommand(requestId, homeownerId);
            var request = await requestCommandService.Handle(command);

            if (request is null)
                return NotFound(new { message = $"Request {requestId} not found" });

            var requestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(request);
            logger.LogInformation("Service request {RequestId} confirmed - auto-assignment triggered", requestId);

            return Ok(requestResource);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error confirming request");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    #endregion

    #region Query Endpoints

    /// <summary>
    /// Get a specific service request by ID
    /// </summary>
    [HttpGet("{requestId:guid}")]
    [SwaggerOperation(
        Summary = "Get Request by ID",
        Description = "Retrieves a specific service request. Validates homeowner ownership.",
        OperationId = "GetRequestById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request found", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Request not found or unauthorized")]
    public async Task<IActionResult> GetRequestById(Guid requestId, [FromQuery] Guid homeownerId)
    {
        var query = new GetServiceRequestByIdQuery(requestId, homeownerId);
        var request = await requestQueryService.Handle(query);

        if (request is null)
            return NotFound(new { message = $"Request {requestId} not found or you don't have access" });

        var resource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(request);
        return Ok(resource);
    }

    /// <summary>
    /// Get all service requests for a homeowner
    /// </summary>
    [HttpGet("homeowner/{homeownerId:guid}")]
    [SwaggerOperation(
        Summary = "Get Requests by Homeowner",
        Description = "Retrieves all service requests for a specific homeowner.",
        OperationId = "GetRequestsByHomeowner")]
    [SwaggerResponse(StatusCodes.Status200OK, "Requests retrieved", typeof(List<ServiceRequestListResource>))]
    public async Task<IActionResult> GetRequestsByHomeowner(Guid homeownerId)
    {
        var query = new GetAllRequestsByHomeownerQuery(homeownerId);
        var requests = await requestQueryService.Handle(query);

        var resources = requests.Select(r => 
            ServiceRequestResourceFromEntityAssembler.ToListResourceFromEntity(r));
        return Ok(resources);
    }

    /// <summary>
    /// Get requests by status for a homeowner
    /// </summary>
    [HttpGet("homeowner/{homeownerId:guid}/by-status/{status}")]
    [SwaggerOperation(
        Summary = "Get Requests by Status",
        Description = "Filters service requests by status for a homeowner.",
        OperationId = "GetRequestsByStatus")]
    [SwaggerResponse(StatusCodes.Status200OK, "Filtered requests retrieved", typeof(List<ServiceRequestListResource>))]
    public async Task<IActionResult> GetRequestsByStatus(Guid homeownerId, string status)
    {
        var query = new GetRequestsByStatusQuery(homeownerId, status);
        var requests = await requestQueryService.Handle(query);

        var resources = requests.Select(r =>
            ServiceRequestResourceFromEntityAssembler.ToListResourceFromEntity(r));
        return Ok(resources);
    }

    /// <summary>
    /// Get all pending assignment requests (Admin/System)
    /// </summary>
    [HttpGet("pending-assignments")]
    [SwaggerOperation(
        Summary = "Get Pending Assignments",
        Description = "Retrieves all requests waiting for assignment (Admin endpoint).",
        OperationId = "GetPendingAssignments")]
    [SwaggerResponse(StatusCodes.Status200OK, "Pending requests retrieved", typeof(List<ServiceRequestListResource>))]
    public async Task<IActionResult> GetPendingAssignments()
    {
        var query = new GetPendingAssignmentRequestsQuery();
        var requests = await requestQueryService.Handle(query);

        var resources = requests.Select(r =>
            ServiceRequestResourceFromEntityAssembler.ToListResourceFromEntity(r));
        return Ok(resources);
    }

    #endregion

    #region Cancel Request

    /// <summary>
    /// Cancel a service request
    /// </summary>
    [HttpPost("{requestId:guid}/cancel")]
    [SwaggerOperation(
        Summary = "Cancel Request",
        Description = "Cancels a service request with a reason. Cannot cancel if already assigned.",
        OperationId = "CancelRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "Request cancelled successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Cannot cancel (e.g., already assigned)")]
    public async Task<IActionResult> CancelRequest(
        Guid requestId,
        [FromQuery] Guid homeownerId,
        [FromBody] string reason)
    {
        try
        {
            var command = new CancelServiceRequestCommand(requestId, homeownerId, reason);
            await requestCommandService.Handle(command);

            logger.LogInformation("Request {RequestId} cancelled: {Reason}", requestId, reason);
            return Ok(new { message = "Request cancelled successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error cancelling request");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    #endregion
}

