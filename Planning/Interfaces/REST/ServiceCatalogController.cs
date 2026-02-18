using System.Net.Mime;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST;

[ApiController]
[Route("api/v1/planning/catalogs")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Service Catalog Management Endpoints")]
public class ServiceCatalogController(
    IServiceCatalogCommandService catalogCommandService,
    IServiceCatalogQueryService catalogQueryService,
    ILogger<ServiceCatalogController> logger)
    : ControllerBase
{
    /// <summary>
    /// Create a new service catalog for a technician
    /// </summary>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Service Catalog",
        Description = "Creates a new service catalog for a technician. Each technician can only have one catalog.",
        OperationId = "CreateServiceCatalog")]
    [SwaggerResponse(StatusCodes.Status201Created, "Catalog created successfully", typeof(ServiceCatalogResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request or technician already has a catalog")]
    public async Task<IActionResult> CreateCatalog([FromBody] CreateServiceCatalogResource resource)
    {
        try
        {
            var command = new CreateServiceCatalogCommand(new TechnicianId(resource.TechnicianId));
            var catalog = await catalogCommandService.Handle(command);

            if (catalog is null)
                return BadRequest("Could not create catalog for the specified technician.");

            var catalogResource = ServiceCatalogResourceFromEntityAssembler.ToResourceFromEntity(catalog);
            logger.LogInformation("Catalog created for technician {TechnicianId}", resource.TechnicianId);

            return CreatedAtAction(nameof(GetCatalogByTechnician), 
                new { technicianId = resource.TechnicianId }, catalogResource);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Failed to create catalog");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating catalog");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get service catalog by technician ID
    /// </summary>
    [HttpGet("technician/{technicianId:guid}")]
    [SwaggerOperation(
        Summary = "Get Catalog by Technician",
        Description = "Retrieves the service catalog for a specific technician.",
        OperationId = "GetCatalogByTechnician")]
    [SwaggerResponse(StatusCodes.Status200OK, "Catalog found", typeof(ServiceCatalogResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Catalog not found")]
    public async Task<IActionResult> GetCatalogByTechnician(Guid technicianId)
    {
        var query = new GetServiceCatalogByTechnicianQuery(technicianId);
        var catalog = await catalogQueryService.Handle(query);

        if (catalog is null)
            return NotFound(new { message = $"No catalog found for technician {technicianId}" });

        var resource = ServiceCatalogResourceFromEntityAssembler.ToResourceFromEntity(catalog);
        return Ok(resource);
    }

    /// <summary>
    /// Get active recipes for a technician
    /// </summary>
    [HttpGet("technician/{technicianId:guid}/recipes/active")]
    [SwaggerOperation(
        Summary = "Get Active Recipes",
        Description = "Retrieves all active service recipes for a technician.",
        OperationId = "GetActiveRecipes")]
    [SwaggerResponse(StatusCodes.Status200OK, "Active recipes retrieved", typeof(List<ServiceRecipeResource>))]
    public async Task<IActionResult> GetActiveRecipes(Guid technicianId)
    {
        var query = new GetActiveRecipesByTechnicianQuery(technicianId);
        var recipes = await catalogQueryService.Handle(query);

        var resources = recipes.Select(ServiceCatalogResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Get a specific recipe by ID
    /// </summary>
    [HttpGet("recipes/{recipeId:guid}")]
    [SwaggerOperation(
        Summary = "Get Recipe by ID",
        Description = "Retrieves a specific service recipe by its ID.",
        OperationId = "GetRecipeById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe found", typeof(ServiceRecipeResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found")]
    public async Task<IActionResult> GetRecipeById(Guid recipeId)
    {
        var query = new GetServiceRecipeByIdQuery(recipeId);
        var recipe = await catalogQueryService.Handle(query);

        if (recipe is null)
            return NotFound(new { message = $"Recipe {recipeId} not found" });

        var resource = ServiceCatalogResourceFromEntityAssembler.ToResourceFromEntity(recipe);
        return Ok(resource);
    }

    /// <summary>
    /// Create a new service recipe
    /// </summary>
    [HttpPost("recipes")]
    [SwaggerOperation(
        Summary = "Create Service Recipe",
        Description = "Creates a new service recipe in the technician's catalog.",
        OperationId = "CreateRecipe")]
    [SwaggerResponse(StatusCodes.Status201Created, "Recipe created successfully", typeof(ServiceRecipeResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid recipe data")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Technician does not own the catalog")]
    public async Task<IActionResult> CreateRecipe(
        [FromBody] CreateServiceRecipeResource resource,
        [FromQuery] Guid technicianId)
    {
        try
        {
            var command = CreateServiceRecipeCommandFromResourceAssembler.ToCommandFromResource(
                resource, technicianId);
            var recipe = await catalogCommandService.Handle(command);

            if (recipe is null)
                return BadRequest("Could not create recipe with the provided data.");

            var recipeResource = ServiceCatalogResourceFromEntityAssembler.ToResourceFromEntity(recipe);
            logger.LogInformation("Recipe {RecipeId} created for catalog {CatalogId}", 
                recipe.Id.Id, resource.CatalogId);

            return CreatedAtAction(nameof(GetRecipeById), 
                new { recipeId = recipe.Id.Id }, recipeResource);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Unauthorized attempt to create recipe");
            return Unauthorized(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid recipe data");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating recipe");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing service recipe
    /// </summary>
    [HttpPut("recipes/{recipeId:guid}")]
    [SwaggerOperation(
        Summary = "Update Service Recipe",
        Description = "Updates an existing service recipe.",
        OperationId = "UpdateRecipe")]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe updated successfully", typeof(ServiceRecipeResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Technician does not own the recipe")]
    public async Task<IActionResult> UpdateRecipe(
        Guid recipeId,
        [FromBody] UpdateServiceRecipeResource resource,
        [FromQuery] Guid technicianId)
    {
        try
        {
            var command = UpdateServiceRecipeCommandFromResourceAssembler.ToCommandFromResource(
                resource, recipeId, technicianId);
            var recipe = await catalogCommandService.Handle(command);

            if (recipe is null)
                return NotFound(new { message = $"Recipe {recipeId} not found" });

            var recipeResource = ServiceCatalogResourceFromEntityAssembler.ToResourceFromEntity(recipe);
            logger.LogInformation("Recipe {RecipeId} updated", recipeId);

            return Ok(recipeResource);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating recipe");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    /// <summary>
    /// Deactivate a service recipe
    /// </summary>
    [HttpPost("recipes/{recipeId:guid}/deactivate")]
    [SwaggerOperation(
        Summary = "Deactivate Recipe",
        Description = "Deactivates a service recipe with a reason.",
        OperationId = "DeactivateRecipe")]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe deactivated successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found")]
    public async Task<IActionResult> DeactivateRecipe(
        Guid recipeId,
        [FromQuery] Guid technicianId,
        [FromBody] string reason)
    {
        try
        {
            var command = new DeactivateServiceRecipeCommand(recipeId, technicianId, reason);
            await catalogCommandService.Handle(command);

            logger.LogInformation("Recipe {RecipeId} deactivated", recipeId);
            return Ok(new { message = "Recipe deactivated successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deactivating recipe");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Reactivate a service recipe
    /// </summary>
    [HttpPost("recipes/{recipeId:guid}/reactivate")]
    [SwaggerOperation(
        Summary = "Reactivate Recipe",
        Description = "Reactivates a previously deactivated recipe.",
        OperationId = "ReactivateRecipe")]
    [SwaggerResponse(StatusCodes.Status200OK, "Recipe reactivated successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found")]
    public async Task<IActionResult> ReactivateRecipe(
        Guid recipeId,
        [FromQuery] Guid technicianId)
    {
        try
        {
            var command = new ReactivateServiceRecipeCommand(recipeId, technicianId);
            await catalogCommandService.Handle(command);

            logger.LogInformation("Recipe {RecipeId} reactivated", recipeId);
            return Ok(new { message = "Recipe reactivated successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reactivating recipe");
            return BadRequest(new { message = ex.Message });
        }
    }
}

