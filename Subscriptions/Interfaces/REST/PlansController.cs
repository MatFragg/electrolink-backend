using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;


/// <summary>
/// REST API controller for managing subscription plans.
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/plans")]
[Produces("application/json")]
public class PlanController(IPlanCommandService commandService, IPlanQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Gets all available plans.
    /// </summary>
    /// <returns>A list of plan resources.</returns>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var plans = await queryService.Handle(new GetAllPlansQuery());
        var resources = plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Gets a plan by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the plan.</param>
    /// <returns>The plan resource if found, otherwise Not Found.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var plan = await queryService.Handle(new GetPlanByIdQuery(id));
        return plan is null ? NotFound() : Ok(PlanResourceFromEntityAssembler.ToResourceFromEntity(plan));
    }

    /// <summary>
    /// Creates a new plan.
    /// </summary>
    /// <param name="resource">The plan creation resource.</param>
    /// <returns>The ID of the created plan.</returns>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Plan created successfully.")]
    public async Task<IActionResult> Create([FromBody] CreatePlanResource resource)
    {
        var command = CreatePlanCommandFromResourceAssembler.ToCommandFromResource(resource);
        var id = await commandService.Handle(command);
    
        if (id == Guid.Empty)
            return BadRequest("Failed to create plan.");

        // Fetch adicional to return in response
        var plan = await queryService.Handle(new GetPlanByIdQuery(id));
        if (plan == null)
            return BadRequest("Failed to retrieve created plan.");

        var planResource = PlanResourceFromEntityAssembler.ToResourceFromEntity(plan);
        return CreatedAtAction(nameof(GetById), new { id }, planResource);
    }
    
    /// <summary>
    /// Updates an existing plan.
    /// </summary>
    /// <param name="id">The ID of the plan to update.</param>
    /// <param name="resource">The plan update resource.</param>
    /// <returns>No Content if successful, otherwise Not Found.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePlanResource resource)
    {
        var command = UpdatePlanCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var updatedPlanId = await commandService.Handle(command);
        return updatedPlanId is null ? NotFound() : NoContent();
    }

    
    /// <summary>
    /// Deletes a plan by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the plan to delete.</param>
    /// <returns>No Content if successful, otherwise Not Found.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await commandService.Handle(new DeletePlanCommand(id));
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }
}