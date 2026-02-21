using System.Net.Mime;
using System.Security.Claims;
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
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

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

    // POST api/v1/profiles/me/complete/homeowner
    /*[HttpPost("me/complete/homeowner")]
    [SwaggerOperation(Summary = "Complete profile as homeowner", OperationId = "CompleteAsHomeowner")]
    [ProducesResponseType(typeof(MyProfileResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompleteAsHomeowner(
        [FromBody] CompleteProfileAsHomeownerResource resource)
    {
        var command = CompleteProfileAsHomeownerCommandFromResourceAssembler
            .ToCommand(resource, UserId);

        var profile = await commandService.Handle(command);
        return Ok(MyProfileResourceFromEntityAssembler.ToResource(profile));
    }*/
}


