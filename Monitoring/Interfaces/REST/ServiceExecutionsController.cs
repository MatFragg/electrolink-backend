using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Interfaces.REST;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Monitoring.Interfaces.REST;

[ApiController]
[Route("api/v1/service-executions")]
[Produces("application/json")]
public class ServiceExecutionsController(
    IServiceExecutionCommandService commandService,
    IServiceExecutionQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Creates a new service execution when a service request is accepted. This endpoint is typically called by the Service Requests API after a homeowner accepts a quote and a technician is assigned. It initializes the service execution record with the associated service request details, but does not start the execution yet. The technician will later call the Start endpoint to begin the execution.
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ServiceExecutionResource>> CreateServiceExecution(
        [FromBody] CreateServiceExecutionResource resource)
    {
        try
        {
            var command = CreateServiceExecutionCommandFromResourceAssembler.ToCommandFromResource(resource);
            var execution = await commandService.Handle(command);
            var result = ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution);

            return CreatedAtAction(nameof(GetServiceExecutionById), new { executionId = result.ExecutionId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves the details of a specific service execution by its ID. This endpoint is used to view the current status, technical report, work log, and other relevant information about an ongoing or completed service execution. It can be accessed by both homeowners and technicians to track the progress and review the outcomes of the service.
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns></returns>
    [HttpGet("{executionId}", Name = nameof(GetServiceExecutionById))]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceExecutionResource>> GetServiceExecutionById(string executionId)
    {
        var execution = await queryService.Handle(new GetServiceExecutionByIdQuery(ServiceExecutionId.From(executionId)));
        if (execution is null) return NotFound(new { message = $"ServiceExecution {executionId} not found." });
        return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
    }

    /// <summary>
    /// Retrieves the work log for a specific service execution, which includes a chronological record of all actions taken, photos uploaded, components used, and updates made during the execution. This endpoint provides a detailed view of the service process, allowing homeowners and technicians to review the steps taken and any issues encountered throughout the execution. It can be particularly useful for troubleshooting, quality assurance, and post-service reviews.
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns></returns>
    [HttpGet("{executionId}/work-log")]
    [ProducesResponseType(typeof(WorkLogResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkLogResource>> GetWorkLog(string executionId)
    {
        var execution = await queryService.Handle(new GetServiceExecutionByIdQuery(ServiceExecutionId.From(executionId)));
        if (execution is null) return NotFound(new { message = $"ServiceExecution {executionId} not found." });
        return Ok(ServiceExecutionResourceFromEntityAssembler.ToWorkLogResourceFromEntity(execution));
    }

    /// <summary>
    /// Retrieves a list of all service executions currently assigned to a specific technician. This endpoint allows technicians to view their active workload and manage their ongoing service executions. The response includes basic details about each assigned execution, such as the service request information, current status, and scheduled times, enabling technicians to prioritize and plan their work effectively.
    /// </summary>
    /// <param name="technicianId"></param>
    /// <returns></returns>
    [HttpGet("technicians/{technicianId}/assigned")]
    [ProducesResponseType(typeof(IEnumerable<ServiceExecutionResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceExecutionResource>>> GetAssignedByTechnician(string technicianId)
    {
        var executions = await queryService.Handle(new GetAssignedServicesByTechnicianQuery(TechnicianId.From(technicianId)));
        return Ok(executions.Select(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("homeowners/{homeownerId}/history")]
    [ProducesResponseType(typeof(IEnumerable<ServiceExecutionResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceExecutionResource>>> GetHistoryByHomeowner(string homeownerId)
    {
        var executions = await queryService.Handle(new GetServiceHistoryByHomeownerQuery(HomeownerId.From(homeownerId)));
        return Ok(executions.Select(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("technicians/{technicianId}/history")]
    [ProducesResponseType(typeof(IEnumerable<ServiceExecutionResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceExecutionResource>>> GetHistoryByTechnician(string technicianId)
    {
        var executions = await queryService.Handle(new GetServiceHistoryByTechnicianQuery(TechnicianId.From(technicianId)));
        return Ok(executions.Select(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("homeowners/{homeownerId}/active")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceExecutionResource>> GetActiveByHomeowner(string homeownerId)
    {
        var execution = await queryService.Handle(new GetActiveServiceByHomeownerQuery(HomeownerId.From(homeownerId)));
        if (execution is null) return NotFound(new { message = "No active service found for this homeowner." });
        return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
    }
    
    [HttpPost("start")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceExecutionResource>> Start(
        string executionId, [FromBody] StartServiceExecutionResource resource)
    {
        try
        {
            var technicianId = User.GetTechnicianId();
            var command      = StartServiceExecutionCommandFromResourceAssembler.ToCommandFromResource(executionId, technicianId, resource);
            var execution    = await commandService.Handle(command);
            return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (KeyNotFoundException ex)      { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        catch (UnauthorizedAccessException)  { return Forbid(); }
    }

    [HttpPost("photos")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceExecutionResource>> UploadPhoto(
        string executionId, [FromBody] UploadWorkPhotoResource resource)
    {
        try
        {
            var technicianId = User.GetTechnicianId();
            var command = UploadWorkPhotoCommandFromResourceAssembler.ToCommandFromResource(
                executionId, technicianId, resource);

            var execution = await commandService.Handle(command);
            return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("components")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceExecutionResource>> RecordComponents(
        string executionId, [FromBody] RecordComponentsUsedResource resource)
    {
        try
        {
            var technicianId = User.GetTechnicianId();
            var command = RecordComponentsUsedCommandFromResourceAssembler.ToCommandFromResource(executionId, technicianId, resource);
            var execution = await commandService.Handle(command);
            return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPut("report")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceExecutionResource>> UpdateReport(
        string executionId, [FromBody] UpdateTechnicalReportResource resource)
    {
        try
        {
            var technicianId = User.GetTechnicianId();
            var command = UpdateTechnicalReportCommandFromResourceAssembler.ToCommandFromResource(
                executionId, technicianId, resource);

            var execution = await commandService.Handle(command);
            return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("complete")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ServiceExecutionResource>> Complete(string executionId)
    {
        try
        {
            var technicianId = User.GetTechnicianId();
            var command      = CompleteServiceExecutionCommandAssembler.ToCommand(executionId, technicianId);
            var execution    = await commandService.Handle(command);
            return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (KeyNotFoundException ex)      { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
        catch (UnauthorizedAccessException)  { return Forbid(); }
    }

    [HttpPost("cancel")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceExecutionResource>> Cancel(
        string executionId, [FromBody] CancelServiceExecutionResource resource)
    {
        try
        {
            var actorId = User.GetUserId();
            var command = CancelServiceExecutionCommandFromResourceAssembler.ToCommandFromResource(
                executionId, actorId, resource);

            var execution = await commandService.Handle(command);
            return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("extend-wait")]
    [ProducesResponseType(typeof(ServiceExecutionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceExecutionResource>> ExtendWait(
        string executionId, [FromBody] ExtendWaitTimeResource resource)
    {
        try
        {
            var homeownerId = User.GetHomeOwnerId();
            var command     = ExtendServiceWaitTimeCommandFromResourceAssembler.ToCommandFromResource(
                executionId, homeownerId, resource);

            var execution = await commandService.Handle(command);
            return Ok(ServiceExecutionResourceFromEntityAssembler.ToResourceFromEntity(execution));
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("reviews/homeowner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitHomeownerReview(
        string executionId, [FromBody] SubmitReviewResource resource)
    {
        try
        {
            var homeownerId = User.GetHomeOwnerId();
            var command = SubmitHomeownerReviewCommandFromResourceAssembler.ToCommandFromResource(
                executionId, homeownerId, resource);

            await commandService.Handle(command);
            return Ok(new { message = "Review submitted successfully." });
        }
        catch (InvalidOperationException ex)  { return BadRequest(new { message = ex.Message }); }
        catch (ArgumentOutOfRangeException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("reviews/technician")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitTechnicianReview(
        string executionId, [FromBody] SubmitReviewResource resource)
    {
        try
        {
            var technicianId = User.GetTechnicianId();
            var command = SubmitTechnicianReviewCommandFromResourceAssembler.ToCommandFromResource(
                executionId, technicianId, resource);

            await commandService.Handle(command);
            return Ok(new { message = "Review submitted successfully." });
        }
        catch (InvalidOperationException ex)  { return BadRequest(new { message = ex.Message }); }
        catch (ArgumentOutOfRangeException ex) { return BadRequest(new { message = ex.Message }); }
    }
}