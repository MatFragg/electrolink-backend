// csharp
using System.Net.Mime;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST;

[ApiController]
[Route("api/v1/technicians")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Technician Inventory Endpoints")]
public class TechnicianInventoriesController(ITechnicianInventoryCommandService inventoryCommandService, ITechnicianInventoryQueryService inventoryQueryService) : ControllerBase
{
    [HttpPost("{technicianId}/inventory")]
    [SwaggerOperation(Summary = "Create technician inventory", OperationId = "CreateTechnicianInventory")]
    [SwaggerResponse(StatusCodes.Status201Created, "Inventory created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Could not create inventory")]
    public async Task<IActionResult> CreateTechnicianInventory(string technicianId)
    {
        var command = new CreateTechnicianInventoryCommand(TechnicianId.From(technicianId));
        var inventory = await inventoryCommandService.Handle(command);
        if (inventory is null) return BadRequest("Could not create inventory.");

        var response = TechnicianInventoryResourceFromEntityAssembler.ToResourceFromEntity(inventory);
        return CreatedAtAction(nameof(GetInventoryByTechnicianId), new { technicianId }, response);
    }

    [HttpPost("{technicianId}/inventory/stock-items")]
    [SwaggerOperation(Summary = "Add stock item to technician inventory", OperationId = "AddStockItemToInventory")]
    [SwaggerResponse(StatusCodes.Status200OK, "Stock item added successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to add stock item")]
    public async Task<IActionResult> AddStockItemToInventory(string technicianId, [FromBody] AddStockToInventoryResource resource)
    {
        var command = AddStockToInventoryCommandFromResourceAssembler.ToCommandFromResource(resource, technicianId);
        var inventory = await inventoryCommandService.Handle(command);
        if (inventory is null) return BadRequest();

        return Ok();
    }

    [HttpPut("{technicianId}/inventory/{componentId}")]
    [SwaggerOperation(Summary = "Update component stock in technician inventory", OperationId = "UpdateComponentStock")]
    [SwaggerResponse(StatusCodes.Status200OK, "Inventory updated", typeof(TechnicianInventoryResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Inventory or component not found")]
    public async Task<IActionResult> UpdateComponentStock(
        string technicianId,
        string componentId,
        [FromBody] UpdateComponentStockResource resource)
    {
        var command = UpdateComponentStockCommandFromResourceAssembler.ToCommandFromResource(resource, technicianId, componentId);
        var inventory = await inventoryCommandService.Handle(command);
        if (inventory is null) return NotFound();

        var query = new GetInventoryByTechnicianIdQuery(TechnicianId.From(technicianId));
        var readModel = await inventoryQueryService.Handle(query);
        if (readModel is null) return NotFound();

        return Ok(TechnicianInventoryResourceFromEntityAssembler.ToResourceFromReadModel(readModel));
    }

    [HttpDelete("{technicianId}/inventory/{componentId}")]
    [SwaggerOperation(Summary = "Remove component from technician inventory", OperationId = "RemoveComponentStock")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Component removed from inventory")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Component or inventory not found")]
    public async Task<IActionResult> RemoveComponentStock(string technicianId, string componentId)
    {
        var command = RemoveComponentStockCommandAssembler.ToCommandFromResource(technicianId, componentId);
        var result = await inventoryCommandService.Handle(command);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpGet("{technicianId}/inventory")]
    [SwaggerOperation(Summary = "Get technician inventory by technician ID", OperationId = "GetInventoryByTechnicianId")]
    [SwaggerResponse(StatusCodes.Status200OK, "Inventory found", typeof(TechnicianInventoryResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Inventory not found")]
    public async Task<IActionResult> GetInventoryByTechnicianId(string technicianId)
    {
        var query = new GetInventoryByTechnicianIdQuery(TechnicianId.From(technicianId));
        var readModel = await inventoryQueryService.Handle(query);
        if (readModel is null) return NotFound();

        return Ok(TechnicianInventoryResourceFromEntityAssembler.ToResourceFromReadModel(readModel));
    }
}
