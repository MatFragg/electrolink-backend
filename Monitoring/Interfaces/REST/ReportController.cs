using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hamcoders.Electrolink.API.Monitoring.Domain.Services;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;
using Hamcoders.Electrolink.API.Monitoring.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hamcoders.Electrolink.API.Monitoring.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[SwaggerTag("Reports endpoints")]
public class ReportsController(
    IReportCommandService reportCommandService,
    IReportQueryService reportQueryService
) : ControllerBase
{
// POST /api/v1/report/{id}
    [HttpPost("{id}/report")]
    [SwaggerOperation(Summary = "Add report", Description = "Adds a report to a service operation.", OperationId = "AddReport")]
    [SwaggerResponse(StatusCodes.Status201Created, "Report added", typeof(Report))]
    public async Task<IActionResult> AddReport(Guid id, [FromBody] CreateReportResource resource)
    {
        var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await reportCommandService.Handle(command);

        var report = await reportQueryService.GetByRequestIdAsync(id);
        return CreatedAtAction(nameof(GetReport), new { id }, report);
    }
    
// GET /api/v1/report/{id}
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get report", Description = "Gets the report for a service operation.", OperationId = "GetReport")]
    [SwaggerResponse(StatusCodes.Status200OK, "Report found", typeof(Report))]
    public async Task<IActionResult> GetReport(Guid id)
    {
        var report = await reportQueryService.GetByRequestIdAsync(id);
        if (report == null) return NotFound();
        return Ok(report);
    }
    
    [HttpPost("{id}/photo")]
    [SwaggerOperation(Summary = "Add Photo", Description = "Adds a photo to a report.", OperationId = "AddPhoto")]
    [SwaggerResponse(StatusCodes.Status201Created, "Photo added", typeof(Report))]
    public async Task<IActionResult> AddPhoto(Guid id, [FromBody] CreateReportPhotoResource resource)
    {
        var command = CreateReportPhotoCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await reportCommandService.Handle(command);

        var report = await reportQueryService.GetByIdWithPhotosAsync(id);
        return CreatedAtAction(nameof(GetReport), new { id }, report);
    }
    
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a report", OperationId = "DeleteReport")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Report deleted")]
    public async Task<IActionResult> DeleteRating(Guid id)
    {
        try
        {
            await reportCommandService.Handle(new DeleteReportCommand(id));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Rating not found");
        }
    }

     [HttpGet]
     [SwaggerOperation(
         Summary = "Get all reports",
         Description = "Returns a list of all reports in the system.",
         OperationId = "GetAllReports")]
     [SwaggerResponse(200, "List of reports returned successfully")]
     public async Task<IActionResult> GetAllReports() {
         var result = await reportQueryService.Handle(new GetAllReportsQuery());
         return Ok(result);
     }
}
