
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Application.ACL;

/// <summary>
/// Facade for the profiles context
/// </summary>
public class ProfilesContextFacade(
    IProfileQueryService profileQueryService, IProfileRepository profileRepository
) : IProfilesContextFacade
{
    public async Task<string?> GetTechnicianIdByUserIdAsync(string userId)
    {
        var profile = await profileRepository.FindByUserIdAsync(UserId.From(userId));

        if (profile is null || profile.BusinessRole != EBusinessRole.Technician || profile.Technician is null)
            return null;

        return profile.Technician.TechnicianId.Value; 
    }
    
    public async Task<(string technicianId, string userId)?> GetTechnicianInfoByProfileIdAsync(string userId)
    {
        var profile = await profileRepository.FindByUserIdAsync(UserId.From(userId));
        if (profile is null || profile.BusinessRole != EBusinessRole.Technician || profile.Technician is null)
            return null;

        return (profile.Technician.TechnicianId.Value, profile.UserId.Value);
    }

    public async Task<bool> ExistsTechnicianProfileByUserIdAsync(string userId)
    {
        var profile = await profileRepository.FindByUserIdAsync(UserId.From(userId));
        return profile is not null && profile.BusinessRole == EBusinessRole.Technician;
    }
    
    public async Task<string?> GetProfileFullNameAsync(string profileId)
    {
        var profile = await profileRepository.FindByIdAsync(ProfileId.From(profileId));
        
        if (profile?.PersonalData is null)
            return null;
        
        return profile.PersonalData.FullName;
    }
    
    public async Task<string?> GetProfilePhoneAsync(string profileId)
    {
        var profile = await profileRepository.FindByIdAsync(ProfileId.From(profileId));
        return profile?.PersonalData?.PhoneNumber.Value;
    }
    
    public async Task<string?> GetProfileRoleAsync(string profileId)
    {
        var profile = await profileRepository.FindByIdAsync(ProfileId.From(profileId));
        return profile?.BusinessRole.ToString();
    }

    public async Task<bool> ProfileExistsAsync(string profileId)
    {
        var profile = await profileRepository.FindByIdAsync(ProfileId.From(profileId));
        return profile is not null;
    }
    
    public async Task<bool> IsHomeownerActiveAsync(string homeownerId)
    {
        return await profileRepository.IsHomeownerActiveAsync(HomeownerId.From(homeownerId));
    }

    public async Task<IEnumerable<(string technicianId, string profileId, string fullName)>> GetTechniciansInAreaAsync(double latitude, double longitude)
        => await profileQueryService.Handle(new GetTechniciansInAreaQuery(latitude, longitude));

    public async Task<(string technicianId, double serviceAreaLat, double serviceAreaLon, int experienceYears, IEnumerable<string> specialties)> GetTechnicianDetailsAsync(string technicianId)
    {
        var profile = await profileRepository.FindByTechnicianIdAsync(TechnicianId.From(technicianId));

        if (profile?.Technician is null)
            throw new InvalidOperationException($"Technician with ID {technicianId} not found.");

        return (
            technicianId,
            profile.Technician.ServiceArea.CenterLatitude,
            profile.Technician.ServiceArea.CenterLongitude,
            profile.Technician.ExperienceYears,
            profile.Technician.Specialties.Select(s => s.ToString())
        );
    }

    public async Task<IEnumerable<string>> GetTechnicianSpecialtiesAsync(string technicianId)
    {
        var profile = await profileRepository.FindByTechnicianIdAsync(TechnicianId.From(technicianId));

        if (profile?.Technician is null)
            return [];

        return profile.Technician.Specialties.Select(s => s.ToString());
    }

    public Task<(string ProfileId, string ProfileStatus, string? BusinessRole, string? RoleSubjectId)?> GetProfileClaimsAsync(string userId)
    {
        return profileQueryService.Handle(new GetProfileClaimsQuery(userId));
    }
}