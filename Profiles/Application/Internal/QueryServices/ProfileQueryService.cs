using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.QueryServices;

/// <summary>
/// Application-level query service for Profiles.
/// </summary>
public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query)
        => await profileRepository.ListWithDetailsAsync();

    public async Task<IEnumerable<Profile>> Handle(GetProfilesByRoleQuery query)
    {
        var allProfiles = await profileRepository.ListAsync();
        return allProfiles.Where(p => p.Role == query.Role);
    }

    public async Task<Profile?> Handle(GetProfileByIdQuery query)
    {
        return await profileRepository.FindByProfileIdAsync(query.Id);
    }

    public async Task<Profile?> Handle(GetProfileByEmailQuery query)
    {
      return await profileRepository.FindByEmailAsync(query.Email.Address);
    }
}
