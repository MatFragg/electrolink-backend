using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.QueryServices;

/// <summary>
/// Application-level query service for Profiles.
/// </summary>
public class ProfileQueryService(IProfileRepository profileRepository, ILogger<ProfileQueryService> logger) : IProfileQueryService
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

    public async  Task<Profile?> Handle(GetProfileInfoByUserIdQuery query)
    {
        return await profileRepository.FindByUserIdAsync(query.UserId);
    }

    public async Task<PortfolioItem?> Handle(GetPortfolioItemByWorkIdQuery query)
    {
        var profile = await profileRepository.FindByProfileIdAsync(query.ProfileId);
        if (profile is null) { logger.LogWarning($"Profile not found with ID: {query.ProfileId}."); return null; }
        if (profile.Technician == null) { logger.LogWarning($"Profile ID {query.ProfileId} is not a technician, no portfolio."); return null; }
        return profile.Technician.PortfolioItems.FirstOrDefault(item => item.WorkId == query.WorkId);
    }

    public async Task<IReadOnlyList<PortfolioItem>> Handle(GetAllPortfolioItemsByProfileIdQuery query)
    {
        var profile = await profileRepository.FindByProfileIdAsync(query.ProfileId);
        if (profile is null) { logger.LogWarning($"Profile not found with ID: {query.ProfileId}. Returning empty list."); return new List<PortfolioItem>(); }
        if (profile.Technician == null) { logger.LogWarning($"Profile ID {query.ProfileId} is not a technician, no portfolio. Returning empty list."); return new List<PortfolioItem>(); }
        return profile.Technician.PortfolioItems;
    }
}
