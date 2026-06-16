using System.Security.Claims;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST;

[ApiController]
[Produces("application/json")]
public abstract class BaseProfileController(
    IProfileCommandService commandService,
    IProfileQueryService queryService) : ControllerBase
{
    protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    protected async Task<string?> GetProfileIdAsync()
    {
        var profile = await queryService.Handle(new GetMyProfileQuery(UserId));
        return profile?.ProfileId.Value;
    }

    protected IProfileCommandService CommandService => commandService;
    protected IProfileQueryService QueryService => queryService;
}