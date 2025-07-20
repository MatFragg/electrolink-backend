using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST;

[ApiController]
[Route("api/v1/owners/{ownerId:guid}/properties")]
[Produces("application/json")]
[SwaggerTag("Available Property Endpoints")]
public class PropertiesController(IPropertyCommandService propertyCommandService, IPropertyQueryService propertyQueryService) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of properties for a given owner, with optional filtering.
    /// </summary>
    /// <param name="ownerId">The owner's unique identifier (from the URL).</param>
    /// <param name="city">Optional: filter by city.</param>
    /// <param name="district">Optional: filter by district.</param>
    /// <param name="region">Optional: filter by region.</param>
    /// <param name="street">Optional: filter by street.</param>
    /// <returns>A list of properties matching the criteria.</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all properties by owner",
        Description = "Returns a list of properties for a specific owner, with optional filters by city, district, region, or street.",
        OperationId = "GetAllPropertiesByOwner"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of properties", typeof(IEnumerable<PropertyResource>))]
    public async Task<IActionResult> GetAllPropertiesByOwner(
        Guid ownerId, 
        [FromQuery] string? city, 
        [FromQuery] string? district, 
        [FromQuery] string? region, 
        [FromQuery] string? street)
    {
        var query = new GetAllPropertiesByOwnerIdQuery(ownerId, city, district, region, street);
        var properties = await propertyQueryService.Handle(query);

        var resources = properties.Select(PropertyResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    /// <summary>
    /// Creates a new property for a given owner.
    /// </summary>
    /// <param name="ownerId">The owner's unique identifier (from the URL).</param>
    /// <param name="resource">The resource containing property details.</param>
    /// <returns>The created property resource, or 400 if creation failed.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create property",
        Description = "Creates a new property for a specific owner.",
        OperationId = "CreateProperty"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Property created", typeof(PropertyResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Property could not be created")]
    public async Task<IActionResult> CreateProperty(Guid ownerId, [FromBody] CreatePropertyResource resource)
    {
        var createPropertyCommand = CreatePropertyCommandFromResourceAssembler.ToCommandFromResource(resource, ownerId);

        var property = await propertyCommandService.Handle(createPropertyCommand);
        if (property is null) return BadRequest();
        var propertyResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
    
        return CreatedAtAction(nameof(GetPropertyById), new { ownerId, propertyId = propertyResource.Id }, propertyResource);
    }

    /// <summary>
    /// Retrieves a property by its unique identifier for a given owner.
    /// </summary>
    /// <param name="ownerId">The owner's unique identifier (from the URL).</param>
    /// <param name="propertyId">The property's unique identifier.</param>
    /// <returns>The property resource if found, or 404 if not found.</returns>
    [HttpGet("{propertyId:guid}")]
    [SwaggerOperation(
        Summary = "Get property by Id",
        Description = "Returns a property by its unique identifier for a specific owner.",
        OperationId = "GetPropertyById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Property found", typeof(PropertyResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Property not found")]
    public async Task<IActionResult> GetPropertyById(Guid ownerId, Guid propertyId)
    {
        var getPropertyByIdQuery = new GetPropertyByIdQuery(propertyId, ownerId);
        var property = await propertyQueryService.Handle(getPropertyByIdQuery);
        if (property == null) return NotFound();
        
        var propertyResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
        return Ok(propertyResource);
    }
    
    /// <summary>
    /// Updates an existing property.
    /// </summary>
    /// <param name="propertyId">The property's unique identifier.</param>
    /// <param name="resource">The resource containing updated details.</param>
    /// <returns>The updated property resource, or 404 if not found.</returns>
    [HttpPut("{propertyId:guid}")]
    [SwaggerOperation(
        Summary = "Update property",
        Description = "Updates an existing property by its unique identifier.",
        OperationId = "UpdateProperty"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Property updated", typeof(PropertyResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Property not found")]
    public async Task<IActionResult> UpdateProperty(Guid propertyId, [FromBody] UpdatePropertyResource resource)
    {
        var updateCommand = UpdatePropertyCommandFromResourceAssembler.ToCommandFromResource(resource, propertyId);
    
        var property = await propertyCommandService.Handle(updateCommand);
        if (property is null)
            return NotFound("Propiedad no encontrada");

        var responseResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
        return Ok(responseResource);
    }

    /// <summary>
    /// Deletes a property by its unique identifier.
    /// </summary>
    /// <param name="propertyId">The property's unique identifier.</param>
    /// <returns>No content if deleted, or 404 if not found.</returns>
    [HttpDelete("{propertyId:guid}")]
    [SwaggerOperation(
        Summary = "Delete property",
        Description = "Deletes a property by its unique identifier.",
        OperationId = "DeleteProperty"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Property deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Property not found")]
    public async Task<IActionResult> DeleteProperty(Guid propertyId)
    {
        var deleteCommand = new DeletePropertyCommand(propertyId);
    
        var result = await propertyCommandService.Handle(deleteCommand);
    
        if (!result)
            return NotFound("Propiedad no encontrada");
    
        return NoContent();
    }

    /// <summary>
    /// Updates the address of an existing property.
    /// </summary>
    /// <param name="propertyId">The property's unique identifier.</param>
    /// <param name="resource">The resource containing the new address.</param>
    /// <returns>The updated property resource, or 404 if not found.</returns>
    [HttpPut("{propertyId:guid}/address")]
    [SwaggerOperation(
        Summary = "Update property address",
        Description = "Updates the address of an existing property.",
        OperationId = "UpdatePropertyAddress"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Property address updated", typeof(PropertyResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Property not found")]
    public async Task<IActionResult> UpdatePropertyAddress(Guid propertyId, [FromBody] UpdateAddressResource resource)
    {
        var command = new UpdatePropertyAddressCommand(propertyId, resource.NewAddress);
    
        var property = await propertyCommandService.Handle(command);
        if (property is null)
            return NotFound("Propiedad no encontrada");

        var responseResource = PropertyResourceFromEntityAssembler.ToResourceFromEntity(property);
        return Ok(responseResource);
    }
}