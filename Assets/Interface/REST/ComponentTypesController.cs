using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST;

/// <summary>
/// Controller for managing component types.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")] 
[SwaggerTag("Component Types Management")]
public class ComponentTypesController(
    IComponentTypeCommandService componentTypeCommandService,
    IComponentTypeQueryService componentTypeQueryService) : ControllerBase
{
    /// <summary>
    /// Creates a new component type.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Create Component Type", OperationId = "CreateComponentType")]
    [SwaggerResponse(StatusCodes.Status201Created, "Component type created", typeof(ComponentTypeResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Component type could not be created")]
    public async Task<IActionResult> CreateComponentType([FromBody] CreateComponentTypeResource resource)
    {
        var command = CreateComponentTypeCommandFromResourceAssembler.ToCommandFromResource(resource);
        var componentType = await componentTypeCommandService.Handle(command);
        if (componentType is null) return BadRequest();

        var responseResource = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return CreatedAtAction(nameof(GetComponentTypeById), new { typeId = responseResource.Id }, responseResource);
    }

    /// <summary>
    /// Gets a component type by its unique identifier.
    /// </summary>
    [HttpGet("{typeId:int}", Name = nameof(GetComponentTypeById))] // -> Ruta: GET /api/v1/componenttypes/{typeId}
    [SwaggerOperation(Summary = "Get Component Type by Id", OperationId = "GetComponentTypeById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Component type found", typeof(ComponentTypeResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component type not found")]
    public async Task<IActionResult> GetComponentTypeById(int typeId)
    {
        var query = new GetComponentTypeByIdQuery(typeId);
        var componentType = await componentTypeQueryService.Handle(query);
        if (componentType is null) return NotFound();

        var resource = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return Ok(resource);
    }

    /// <summary>
    /// Gets a list of all component types.
    /// </summary>
    [HttpGet] 
    [SwaggerOperation(Summary = "Get All Component Types", OperationId = "GetAllComponentTypes")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of component types retrieved", typeof(IEnumerable<ComponentTypeResource>))]
    public async Task<IActionResult> GetAllComponentTypes()
    {
        var query = new GetAllComponentTypesQuery();
        var componentTypes = await componentTypeQueryService.Handle(query);
        var resources = componentTypes.Select(ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Updates an existing component type.
    /// </summary>
    [HttpPut("{typeId:int}")]
    [SwaggerOperation(Summary = "Update Component Type", OperationId = "UpdateComponentType")]
    [SwaggerResponse(StatusCodes.Status200OK, "Component type updated", typeof(ComponentTypeResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component type not found")]
    public async Task<IActionResult> UpdateComponentType(int typeId, [FromBody] UpdateComponentTypeResource resource)
    {
        var command = new UpdateComponentTypeCommand(typeId, resource.Name, resource.Description);
        var componentType = await componentTypeCommandService.Handle(command);
        if (componentType is null) return NotFound("Component Type not found");

        var responseResource = ComponentTypeResourceFromEntityAssembler.ToResourceFromEntity(componentType);
        return Ok(responseResource);
    }

    /// <summary>
    /// Deletes a component type by its unique identifier.
    /// </summary>
    [HttpDelete("{typeId:int}")]
    [SwaggerOperation(Summary = "Delete Component Type", OperationId = "DeleteComponentType")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Component type deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component type not found")]
    public async Task<IActionResult> DeleteComponentType(int typeId)
    {
        var command = new DeleteComponentTypeCommand(typeId);
        var result = await componentTypeCommandService.Handle(command);
        if (!result) return NotFound("Component Type not found");
        
        return NoContent();
    }
}