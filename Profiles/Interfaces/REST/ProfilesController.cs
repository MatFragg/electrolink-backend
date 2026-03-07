using System.Net.Mime;
using System.Security.Claims;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Profile endpoints")]
public class ProfilesController(
    IProfileCommandService commandService,
    IProfileQueryService queryService) : ControllerBase
{
    // ── Helpers ──────────────────────────────────────────────────────────
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    /// <summary>
    /// Resuelve el ProfileId del usuario autenticado usando su UserId del JWT.
    /// Internamente llama a GetMyProfileQuery. Retorna null si el perfil no existe.
    /// </summary>
    private async Task<string?> GetProfileIdAsync()
    {
        var profile = await queryService.Handle(new GetMyProfileQuery(UserId));
        return profile?.ProfileId.Value;
    }

    // ── EXISTENTES ────────────────────────────────────────────────────────

    // GET api/v1/profiles/me
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Get my profile", OperationId = "GetMyProfile")]
    [ProducesResponseType(typeof(MyProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyProfile()
    {
        var profile = await queryService.Handle(new GetMyProfileQuery(UserId));
        if (profile is null) return NotFound(new { message = "Profile not found." });
        return Ok(MyProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
    }

    // POST api/v1/profiles/me/complete/technician
    [HttpPost("me/complete/technician")]
    [SwaggerOperation(Summary = "Complete profile as technician", OperationId = "CompleteAsTechnician")]
    [ProducesResponseType(typeof(MyProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CompleteAsTechnician(
        [FromBody] CompleteProfileAsTechnicianResource resource)
    {
        var command = CompleteProfileAsTechnicianCommandFromResourceAssembler
            .ToCommandFromResource(resource, UserId);
        var profile = await commandService.Handle(command);
        return Ok(MyProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
    }

    // ── ANTES COMENTADO ───────────────────────────────────────────────────

    // POST api/v1/profiles/me/complete/homeowner
    [HttpPost("me/complete/homeowner")]
    [SwaggerOperation(Summary = "Complete profile as homeowner", OperationId = "CompleteAsHomeowner")]
    [ProducesResponseType(typeof(MyProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CompleteAsHomeowner(
        [FromBody] CompleteProfileAsHomeownerResource resource)
    {
        try
        {
            var command = CompleteProfileAsHomeownerCommandFromResourceAssembler
                .ToCommandFromResource(resource, UserId);
            var profile = await commandService.Handle(command);
            return Ok(MyProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
        }
        catch (InvalidProfileStatusException ex)                  { return Conflict(new { message = ex.Message }); }
        catch (DniAlreadyInUseException ex)                       { return Conflict(new { message = ex.Message }); }
        catch (AtLeastOneNotificationChannelRequiredException ex) { return BadRequest(new { message = ex.Message }); }
        catch (ArgumentException ex)                              { return NotFound(new { message = ex.Message }); }
    }

    // ── NUEVOS ────────────────────────────────────────────────────────────

    // GET api/v1/profiles/me/status
    [HttpGet("me/status")]
    [SwaggerOperation(Summary = "Get profile status", OperationId = "GetProfileStatus")]
    [ProducesResponseType(typeof(ProfileStatusResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileStatus()
    {
        var readModel = await queryService.Handle(new GetProfileStatusQuery(UserId));
        if (readModel is null) return NotFound(new { message = "Profile not found." });
        return Ok(ProfileStatusResourceFromReadModelAssembler.ToResourceFromReadModel(readModel));
    }

    // PATCH api/v1/profiles/me/personal-data
    [HttpPatch("me/personal-data")]
    [SwaggerOperation(Summary = "Update personal data", OperationId = "UpdatePersonalData")]
    [ProducesResponseType(typeof(MyProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdatePersonalData(
        [FromBody] UpdatePersonalDataResource resource)
    {
        try
        {
            var profileId = await GetProfileIdAsync();
            if (profileId is null) return NotFound(new { message = "Profile not found." });

            var command = UpdatePersonalDataCommandFromResourceAssembler
                .ToCommandFromResource(resource, profileId, UserId);
            var profile = await commandService.Handle(command);
            return Ok(MyProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
        }
        catch (UnauthorizedProfileAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        catch (InvalidProfileStatusException ex)      { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)                  { return NotFound(new { message = ex.Message }); }
    }

    // PATCH api/v1/profiles/me/technician
    [HttpPatch("me/technician")]
    [SwaggerOperation(Summary = "Update technician data", OperationId = "UpdateTechnicianData")]
    [ProducesResponseType(typeof(MyProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTechnicianData(
        [FromBody] UpdateTechnicianDataRequest resource)
    {
        try
        {
            var profileId = await GetProfileIdAsync();
            if (profileId is null) return NotFound(new { message = "Profile not found." });

            var command = UpdateTechnicianDataCommandFromResourceAssembler
                .ToCommandFromResource(resource, profileId, UserId);
            var profile = await commandService.Handle(command);
            return Ok(MyProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
        }
        catch (UnauthorizedProfileAccessException ex)   { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        catch (InvalidBusinessRoleException ex)         { return BadRequest(new { message = ex.Message }); }
        catch (AtLeastOneSpecialtyRequiredException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidProfileStatusException ex)        { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)                    { return NotFound(new { message = ex.Message }); }
    }

    // PATCH api/v1/profiles/me/homeowner
    [HttpPatch("me/homeowner")]
    [SwaggerOperation(Summary = "Update homeowner preferences", OperationId = "UpdateHomeownerPreferences")]
    [ProducesResponseType(typeof(MyProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateHomeownerPreferences(
        [FromBody] UpdateHomeownerPreferencesResource resource)
    {
        try
        {
            var profileId = await GetProfileIdAsync();
            if (profileId is null) return NotFound(new { message = "Profile not found." });

            var command = UpdateHomeownerPreferencesCommandFromResourceAssembler
                .ToCommandFromResource(resource, profileId, UserId);
            var profile = await commandService.Handle(command);
            return Ok(MyProfileResourceFromEntityAssembler.ToResourceFromEntity(profile));
        }
        catch (UnauthorizedProfileAccessException ex)             { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        catch (InvalidBusinessRoleException ex)                   { return BadRequest(new { message = ex.Message }); }
        catch (AtLeastOneNotificationChannelRequiredException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidProfileStatusException ex)                  { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)                              { return NotFound(new { message = ex.Message }); }
    }

    // DELETE api/v1/profiles/me
    [HttpDelete("me")]
    [SwaggerOperation(Summary = "Deactivate profile", OperationId = "DeactivateProfile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeactivateProfile(
        [FromBody] DeactivateProfileResource resource)
    {
        try
        {
            var profileId = await GetProfileIdAsync();
            if (profileId is null) return NotFound(new { message = "Profile not found." });

            await commandService.Handle(new DeactivateProfileCommand(
                ProfileId: profileId,
                UserId:    UserId,
                Reason:    resource.Reason,
                Notes:     resource.Notes));
            return NoContent();
        }
        catch (UnauthorizedProfileAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        catch (InvalidProfileStatusException ex)      { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)                  { return NotFound(new { message = ex.Message }); }
    }

    // POST api/v1/profiles/me/reactivate
    [HttpPost("me/reactivate")]
    [SwaggerOperation(Summary = "Reactivate profile", OperationId = "ReactivateProfile")]
    [ProducesResponseType(typeof(MyProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReactivateProfile()
    {
        try
        {
            var profileId = await GetProfileIdAsync();
            if (profileId is null) return NotFound(new { message = "Profile not found." });

            await commandService.Handle(new ReactivateProfileCommand(
                ProfileId: profileId,
                UserId:    UserId));

            var profile = await queryService.Handle(new GetMyProfileQuery(UserId));
            return Ok(MyProfileResourceFromEntityAssembler.ToResourceFromEntity(profile!));
        }
        catch (UnauthorizedProfileAccessException ex) { return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message }); }
        catch (InvalidProfileStatusException ex)      { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)                  { return NotFound(new { message = ex.Message }); }
    }
}
