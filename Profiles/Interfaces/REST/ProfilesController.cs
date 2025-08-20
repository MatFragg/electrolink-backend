using System.Net.Mime;
using System.Security.Claims;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Profile Endpoints.")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService, ILogger<ProfilesController> logger)
    : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Profile",
        Description = "Create a new profile (Homeowner or Technician). No authentication required.",
        OperationId = "CreateProfile")]
    [SwaggerResponse(StatusCodes.Status201Created, "The profile was created.", typeof(ProfileResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The profile was not created.")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileResource resource)
    {
        try
        {
            var userId = TryGetUserIdFromClaims();
        
            var command = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(
                resource, 
                userId ?? 0);  
            
            var profile = await profileCommandService.Handle(command);
        
            if (profile is null)
                return BadRequest("The profile could not be created with the provided data.");

            var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        
            logger.LogInformation("Profile successfully created with ID {ProfileId}", profile.Id);
        
            return CreatedAtAction(nameof(GetProfileById), new { profileId = profile.Id }, profileResource);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating profile");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected internal error occurred.", error = ex.Message });
        }
    }

    [HttpGet("{profileId:int}")]
    [SwaggerOperation(Summary = "Get Profile by Id", OperationId = "GetProfileById")]
    public async Task<IActionResult> GetProfileById(int profileId)
    {
        var query = new GetProfileByIdQuery(profileId);
        var profile = await profileQueryService.Handle(query);
        if (profile is null) return NotFound();

        var resource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(resource);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get All Profiles", OperationId = "GetAllProfiles")]
    public async Task<IActionResult> GetAllProfiles()
    {
        var query = new GetAllProfilesQuery();
        var profiles = await profileQueryService.Handle(query);
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("by-role/{role}")]
    [SwaggerOperation(Summary = "Get Profiles by Role", OperationId = "GetProfilesByRole")]
    public async Task<IActionResult> GetProfilesByRole(string role)
    {
        if (!Enum.TryParse<Role>(role, true, out var parsedRole))
            return BadRequest("Invalid role.");

        var query = new GetProfilesByRoleQuery(parsedRole);
        var profiles = await profileQueryService.Handle(query);
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("by-email")]
    [SwaggerOperation(Summary = "Get Profile by Email", OperationId = "GetProfileByEmail")]
    public async Task<IActionResult> GetProfileByEmail([FromQuery] string email)
    {
        var query = new GetProfileByEmailQuery(new EmailAddress(email));
        var profile = await profileQueryService.Handle(query);
        if (profile is null) return NotFound();

        var resource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(resource);
    }
    
    
    [Authorize]
    [HttpPost("{profileId}/portfolio")]
    [SwaggerOperation(
        Summary = "Add a new portfolio item",
        Description = "Adds a new portfolio item to a technician's profile. Requires technician role and resource ownership.",
        OperationId = "AddPortfolioItem")]
    [SwaggerResponse(StatusCodes.Status201Created, "Portfolio item created successfully", typeof(Guid))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data or profile not found/not a technician")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User ID in token does not match route ID")]
    public async Task<IActionResult> AddPortfolioItem(int profileId, [FromBody] CreatePortfolioItemResource resource)
    {
        var userIdFromToken = User.FindFirstValue(ClaimTypes.Sid);
        
        if (userIdFromToken == null || !int.TryParse(userIdFromToken, out var parsedUserId))
        {
            return Unauthorized("Authentication failed: User ID claim missing or invalid.");
        }

        if (parsedUserId != profileId)
        {
            return Forbid(); // Autenticado, pero no autorizado para este recurso específico.
        }

        try
        {
            var command = AddPortfolioItemCommandFromResourceAssembler.ToCommandFromResource(profileId, resource);
            var workId = await profileCommandService.Handle(command);

            if (workId == null)
            {
                return BadRequest("La operación no puede completarse. Verifique si el perfil existe y si el usuario es un técnico, o los datos de entrada.");
            }

            return CreatedAtAction(nameof(GetPortfolioItemByWorkId), new { profileId = profileId, workId = workId }, workId);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocurrió un error interno inesperado.", error = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{profileId}/portfolio/{workId}")]
    [SwaggerOperation(
        Summary = "Update a portfolio item",
        Description = "Updates the details of an existing portfolio item in a technician's profile. Requires technician role and resource ownership.",
        OperationId = "UpdatePortfolioItem")]
    [SwaggerResponse(StatusCodes.Status200OK, "Portfolio item updated successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data or profile not found/not a technician")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User ID in token does not match route ID")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Portfolio item not found")]
    public async Task<IActionResult> UpdatePortfolioItem(int profileId, Guid workId, [FromBody] UpdatePortfolioItemResource resource)
    {
        var userIdFromToken = User.FindFirstValue(ClaimTypes.Sid);
        if (userIdFromToken == null || !int.TryParse(userIdFromToken, out var parsedUserId))
        {
            return Unauthorized("Authentication failed: User ID claim missing or invalid.");
        }
        if (parsedUserId != profileId)
        {
            return Forbid();
        }

        try
        {
            // 2. **Llamada al Servicio de Comando:**
            var command = UpdatePortfolioItemDetailsCommandFromResourceAssembler.ToCommandFromResource(profileId, workId, resource);
            var result = await profileCommandService.Handle(command); // Asumimos que devuelve bool

            if (!result)
            {
                return NotFound($"La operación falló: El item de portafolio con WorkId {workId} no fue encontrado, o el perfil no existe/no es técnico.");
            }
            return Ok(new { message = "Item de portafolio actualizado exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocurrió un error interno inesperado.", error = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{profileId}/portfolio/{workId}")]
    [SwaggerOperation(
        Summary = "Delete a portfolio item",
        Description = "Deletes a portfolio item from a technician's profile. Requires technician role and resource ownership.",
        OperationId = "DeletePortfolioItem")]
    [SwaggerResponse(StatusCodes.Status200OK, "Portfolio item deleted successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data or profile not found/not a technician")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User ID in token does not match route ID")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Portfolio item not found")]
    public async Task<IActionResult> DeletePortfolioItem(int profileId, Guid workId)
    {
        var userIdFromToken = User.FindFirstValue(ClaimTypes.Sid);
        if (userIdFromToken == null || !int.TryParse(userIdFromToken, out var parsedUserId))
        {
            return Unauthorized("Authentication failed: User ID claim missing or invalid.");
        }
        if (parsedUserId != profileId)
        {
            return Forbid();
        }

        try
        {
            var command = new RemovePortfolioItemCommand(profileId, workId);
            var result = await profileCommandService.Handle(command); 

            if (!result)
            {
                return NotFound($"La operación falló: El item de portafolio con WorkId {workId} no fue encontrado, o el perfil no existe/no es técnico.");
            }
            return Ok(new { message = "Item de portafolio eliminado exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocurrió un error interno inesperado.", error = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("{profileId}/portfolio/{workId}")]
    [SwaggerOperation(
        Summary = "Get a portfolio item by WorkId",
        Description = "Retrieves a specific portfolio item from a technician's profile by its WorkId.",
        OperationId = "GetPortfolioItemByWorkId")]
    [SwaggerResponse(StatusCodes.Status200OK, "Portfolio item found", typeof(PortfolioItemResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Portfolio item or profile not found")]
    public async Task<IActionResult> GetPortfolioItemByWorkId(int profileId, Guid workId)
    {
        var query = new GetPortfolioItemByWorkIdQuery(profileId, workId);
        var portfolioItem = await profileQueryService.Handle(query);
        if (portfolioItem == null) return NotFound();
        
        var resource = PortfolioItemResourceFromEntityAssembler.ToResourceFromEntity(portfolioItem);
        return Ok(resource);
    }

    [HttpGet("{profileId}/portfolio")]
    [SwaggerOperation(
        Summary = "Get all portfolio items for a technician",
        Description = "Retrieves all portfolio items associated with a technician's profile.",
        OperationId = "GetAllPortfolioItemsByProfileId")]
    [SwaggerResponse(StatusCodes.Status200OK, "Portfolio items found", typeof(IEnumerable<PortfolioItemResource>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Profile not found")]
    public async Task<IActionResult> GetAllPortfolioItemsByProfileId(int profileId)
    {
        var profileExists = await profileQueryService.Handle(new GetProfileByIdQuery(profileId)) != null;
        if (!profileExists) return NotFound($"Profile with ID {profileId} not found.");

        var query = new GetAllPortfolioItemsByProfileIdQuery(profileId);
        var portfolioItems = await profileQueryService.Handle(query);
        
        var resources = portfolioItems.Select(PortfolioItemResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    private int? TryGetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.Sid);

        if (string.IsNullOrEmpty(userIdClaim)) 
            return null;

        if (int.TryParse(userIdClaim, out var userId) && userId > 0) 
            return userId;

        return null;
    }
}


