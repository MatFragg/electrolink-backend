using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
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
[SwaggerTag("Ratings endpoints")]
public class RatingsController(
    IRatingCommandService ratingCommandService,
    IRatingQueryService ratingQueryService
) : ControllerBase
{
    [HttpPost("{id}/rating")]
    [SwaggerOperation(Summary = "Add rating", Description = "Adds a rating to a completed service operation.", OperationId = "AddRating")]
    [SwaggerResponse(StatusCodes.Status201Created, "Rating added", typeof(Rating))]
    public async Task<IActionResult> AddRating(Guid id, [FromBody] CreateRatingResource resource)
    {
        var command = CreateRatingCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        await ratingCommandService.Handle(command);

        var rating = await ratingQueryService.GetByRequestIdAsync(id);
        return CreatedAtAction(nameof(GetRating), new { id }, rating);
    }
    
    // Obtener rating
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get rating", Description = "Gets the rating for a service operation.", OperationId = "GetRating")]
    [SwaggerResponse(StatusCodes.Status200OK, "Rating retrieved", typeof(Rating))]
    public async Task<IActionResult> GetRating(Guid id)
    {
        var rating = await ratingQueryService.GetByRequestIdAsync(id);
        if (rating == null) return NotFound();
        return Ok(rating);
    }
    
    // Ratings de técnico
    [HttpGet("technician/{technicianId}/ratings")]
    [SwaggerOperation(Summary = "Get ratings by technician", OperationId = "GetRatingsByTechnicianId")]
    public async Task<IActionResult> GetByTechnicianId([FromRoute] int technicianId)
    {
        var ratings = await ratingQueryService.GetByTechnicianIdAsync(technicianId);
        return Ok(ratings);
    }
    
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a rating", OperationId = "DeleteRating")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Rating deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Rating not found")]
    public async Task<IActionResult> DeleteRating(Guid id)
    {
        try
        {
            await ratingCommandService.Handle(new DeleteRatingCommand(id));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Rating not found");
        }
    }
    
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a rating", OperationId = "UpdateRating")]
    [SwaggerResponse(StatusCodes.Status200OK, "Rating updated", typeof(Rating))]
    public async Task<IActionResult> UpdateRating(Guid id, [FromBody] UpdateRatingResource resource)
    {
        var command = new UpdateRatingCommand(id, resource.Score, resource.Comment);
        await ratingCommandService.Handle(command);

        return Ok();
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all ratings",
        Description = "Returns a list of all ratings in the system.",
        OperationId = "GetAllRatings")]
    [SwaggerResponse(200, "List of ratings returned successfully")]
    public async Task<IActionResult> GetAllRatings() {
        var result = await ratingQueryService.Handle(new GetAllRatingsQuery());
        return Ok(result);
    }

}
