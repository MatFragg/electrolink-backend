using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/plans")]
public class PlanController : ControllerBase
{
    private readonly IPlanCommandService _commandService;
    private readonly IPlanQueryService _queryService;

    public PlanController(IPlanCommandService commandService, IPlanQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlanResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var plans = await _queryService.Handle(new GetAllPlansQuery());
        var resources = plans.Select(PlanResourceFromEntityAssembler.ToResource);
        return Ok(resources);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PlanResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var plan = await _queryService.Handle(new GetPlanByIdQuery(id));
        return plan is null ? NotFound() : Ok(PlanResourceFromEntityAssembler.ToResource(plan));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePlanResource resource)
    {
        var id = await _commandService.Handle(CreatePlanCommandFromResourceAssembler.ToCommand(resource));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePlanResource resource)
    {
        await _commandService.Handle(UpdatePlanCommandFromResourceAssembler.ToCommand(id, resource));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _commandService.Handle(new DeletePlanCommand(id));
        return NoContent();
    }
}