using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.TechnicianInventories;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST;

/// <summary>
/// Controller for managing technician inventories.
/// </summary>
/// <param name="inventoryCommandService">
/// Command service for technician inventory.
/// </param>
/// <param name="inventoryQueryService">
/// Query service for technician inventory.
/// </param>
/// <param name="componentQueryService">
/// Query service for components.
/// </param>
[ApiController]
[Route("api/v1/technicians")]
[SwaggerTag("Technician Inventory Endpoints")]
public class TechnicianInventoriesController(
    ITechnicianInventoryCommandService inventoryCommandService,
    ITechnicianInventoryQueryService inventoryQueryService,
    IComponentQueryService componentQueryService) : ControllerBase
{
    /// <summary>
    /// Creates an inventory for a specific technician.
    /// </summary>
    /// <param name="technicianId">The unique identifier of the technician.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with status 201 if created successfully, or 400 if failed.
    /// </returns>
    [HttpPost("{technicianId:guid}/inventory")]
    [SwaggerOperation(
        Summary = "Create technician inventory",
        Description = "Creates an inventory for a specific technician.",
        OperationId = "CreateTechnicianInventory"
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Inventory created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Could not create inventory")]
    public async Task<IActionResult> CreateTechnicianInventory(Guid technicianId)
    {
        var command = new CreateTechnicianInventoryCommand(technicianId);
        var inventory = await inventoryCommandService.Handle(command);
        if (inventory is null) return BadRequest("Could not create inventory.");

        return StatusCode(201); 
    }
    
    /// <summary>
    /// Adds a stock item to a technician's inventory.
    /// </summary>
    /// <param name="technicianId">The unique identifier of the technician.</param>
    /// <param name="resource">The stock item data to add.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with status 200 if added successfully, or 400 if failed.
    /// </returns>
    [HttpPost("{technicianId:guid}/inventory/stock-items")]
    [SwaggerOperation(
        Summary = "Add stock item to technician inventory",
        Description = "Adds a stock item to a technician's inventory.",
        OperationId = "AddStockItemToInventory"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Stock item added successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to add stock item")]
    public async Task<IActionResult> AddStockItemToInventory(Guid technicianId, [FromBody] AddStockToInventoryResource resource)
    {
        var command = AddStockToInventoryCommandFromResourceAssembler.ToCommandFromResource(resource, technicianId);
        var inventory = await inventoryCommandService.Handle(command);
        if (inventory is null) return BadRequest();

        return Ok(); 
    }
    
    /// <summary>
    /// Updates the stock quantity of a component in a technician's inventory.
    /// </summary>
    /// <param name="technicianId">The unique identifier of the technician.</param>
    /// <param name="componentId">The unique identifier of the component.</param>
    /// <param name="resource">The stock update data.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with the updated inventory or 404 if not found.
    /// </returns>
    [HttpPut("{technicianId:guid}/inventory/{componentId:guid}")]
    [SwaggerOperation(
        Summary = "Update component stock in technician inventory",
        Description = "Updates the stock quantity of a component in a technician's inventory.",
        OperationId = "UpdateComponentStock"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Inventory updated", typeof(TechnicianInventoryResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Inventory or component not found")]
    public async Task<IActionResult> UpdateComponentStock(
        Guid technicianId, 
        Guid componentId, 
        [FromBody] UpdateComponentStockResource resource)
    {
        var command = UpdateComponentStockCommandFromResourceAssembler.ToCommandFromResource(
            resource, technicianId, componentId);
    
        var inventory = await inventoryCommandService.Handle(command);
        if (inventory is null) return NotFound();
    
        var componentIds = inventory.StockItems.Select(item => item.ComponentId.Id).ToList();
        if (!componentIds.Any())
        {
            var emptyResource = TechnicianInventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory, new Dictionary<ComponentId, string>());
            return Ok(emptyResource);
        } 
        var components = await componentQueryService.Handle(new GetComponentsByIdsQuery(componentIds));
        var componentNames = components.ToDictionary(c => c.Id, c => c.Name);
    
        var inventoryResource = TechnicianInventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory, componentNames);
        return Ok(inventoryResource);
    }

    /// <summary>
    /// Removes a component from a technician's inventory.
    /// </summary>
    /// <param name="technicianId">The unique identifier of the technician.</param>
    /// <param name="componentId">The unique identifier of the component.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with status 204 if removed, or 404 if not found.
    /// </returns>
    [HttpDelete("{technicianId:guid}/inventory/{componentId:guid}")]
    [SwaggerOperation(
        Summary = "Remove component from technician inventory",
        Description = "Removes a component from a technician's inventory.",
        OperationId = "RemoveComponentStock"
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Component removed from inventory")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component or inventory not found")]
    public async Task<IActionResult> RemoveComponentStock(Guid technicianId, Guid componentId)
    {
        var command = RemoveComponentStockCommandAssembler.ToCommand(technicianId, componentId);
    
        var result = await inventoryCommandService.Handle(command);
        if (!result) return NotFound();
    
        return NoContent();
    }
    
    /// <summary>
    /// Gets a technician's inventory by technician ID.
    /// </summary>
    /// <param name="technicianId">The unique identifier of the technician.</param>
    /// <returns>
    /// <see cref="IActionResult"/> with the found inventory or 404 if not found.
    /// </returns>
    [HttpGet("{technicianId:guid}/inventory")]
    [SwaggerOperation(
        Summary = "Get technician inventory by technician ID",
        Description = "Retrieves the inventory for a specific technician.",
        OperationId = "GetInventoryByTechnicianId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Inventory found", typeof(TechnicianInventoryResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Inventory not found")]
    public async Task<IActionResult> GetInventoryByTechnicianId(Guid technicianId)
    {
        var query = new GetInventoryByTechnicianIdQuery(technicianId);
        var inventory = await inventoryQueryService.Handle(query);
        if (inventory is null) return NotFound();

        var componentIds = inventory.StockItems.Select(item => item.ComponentId.Id).ToList();
        if (!componentIds.Any())
        {
            var emptyResource = TechnicianInventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory, new Dictionary<ComponentId, string>());
            return Ok(emptyResource);
        }

        var components = await componentQueryService.Handle(new GetComponentsByIdsQuery(componentIds));

        var componentNames = components.ToDictionary(c => c.Id, c => c.Name);

        var inventoryResource = TechnicianInventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory, componentNames);
        return Ok(inventoryResource);
    }
}