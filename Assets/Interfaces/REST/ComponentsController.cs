using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST;

/// <summary>
/// Controller for managing components.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")] 
[SwaggerTag("Components Management")]
public class ComponentsController(
    IComponentCommandService componentCommandService,
    IComponentQueryService componentQueryService) : ControllerBase
{
    /// <summary>
    /// Creates a new component.
    /// </summary>
    [HttpPost] 
    [SwaggerOperation(Summary = "Create Component", OperationId = "CreateComponent")]
    [SwaggerResponse(StatusCodes.Status201Created, "Component created", typeof(ComponentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Component could not be created")]
    public async Task<IActionResult> CreateComponent([FromBody] CreateComponentResource resource)
    {
        var command = CreateComponentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var component = await componentCommandService.Handle(command);
        if (component is null) return BadRequest();

        var componentResource = ComponentResourceFromEntityAssembler.ToResourceFromEntity(component);
        return Ok(componentResource);

    }
    
    /// <summary>
    /// Gets a component by its unique identifier. (¡NUEVO!)
    /// </summary>
    [HttpGet("{componentId:guid}", Name = nameof(GetComponentById))]
    [SwaggerOperation(Summary = "Get Component by Id", OperationId = "GetComponentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Component found", typeof(ComponentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component not found")]
    public async Task<IActionResult> GetComponentById(string componentId)
    {
        var query = new GetComponentByIdQuery(ComponentId.From(componentId));
        var component = await componentQueryService.Handle(query);
        if (component is null) return NotFound();

        var resource = ComponentResourceFromEntityAssembler.ToResourceFromEntity(component);
        return Ok(resource);
    }

    /// <summary>
    /// Gets a list of all components.
    /// </summary>
    [HttpGet] 
    [SwaggerOperation(Summary = "Get All Components", OperationId = "GetAllComponents")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of components retrieved", typeof(IEnumerable<ComponentResource>))]
    public async Task<IActionResult> GetAllComponents()
    {
        var query = new GetAllComponentsQuery();
        var components = await componentQueryService.Handle(query);
        var resources = components.Select(ComponentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Updates an existing component.
    /// </summary>
    [HttpPut("{componentId}")] 
    [SwaggerOperation(Summary = "Update Component", OperationId = "UpdateComponent")]
    [SwaggerResponse(StatusCodes.Status200OK, "Component updated", typeof(ComponentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component not found")]
    public async Task<IActionResult> UpdateComponent(string componentId, [FromBody] UpdateComponentResource resource)
    {
        var command = UpdateComponentCommandFromResourceAssembler.ToCommandFromResource(resource, componentId);
        var component = await componentCommandService.Handle(command);
        if (component is null) return NotFound("Component not found");

        var responseResource = ComponentResourceFromEntityAssembler.ToResourceFromEntity(component);
        return Ok(responseResource);
    }

    /// <summary>
    /// Deletes a component by its unique identifier.
    /// </summary>
    [HttpDelete("{componentId}")] 
    [SwaggerOperation(Summary = "Delete Component", OperationId = "DeleteComponent")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Component deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component not found")]
    public async Task<IActionResult> DeleteComponent(string componentId)
    {
        var command = new DeleteComponentCommand(ComponentId.From(componentId));
        var result = await componentCommandService.Handle(command);
        if (!result) return NotFound("Component not found");

        return NoContent();
    }
}