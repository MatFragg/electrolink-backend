
using System.Net.Mime;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Profile Endpoints.")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService)
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
        var command = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
        var profile = await profileCommandService.Handle(command);
        if (profile is null) return BadRequest();

        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return CreatedAtAction(nameof(GetProfileById), new { profileId = profile.Id }, profileResource);
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
}


