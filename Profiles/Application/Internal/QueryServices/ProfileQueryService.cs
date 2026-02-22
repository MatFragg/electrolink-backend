using Hampcoders.Electrolink.API.Profiles.Application.Internal.ReadModels;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.QueryServices;

/// <summary>
/// Application-level query service for Profiles.
/// </summary>
public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    public async Task<Profile?> Handle(GetProfileByIdQuery query) => 
        await profileRepository.FindByIdAsync(ProfileId.From(query.ProfileId));
    

    public async Task<Profile?> Handle(GetProfileInfoByUserIdQuery query) =>
        await profileRepository.FindByUserIdAsync(query.UserId);

    public async Task<Profile?> Handle(GetMyProfileQuery query)
    {
        var profile = await profileRepository.FindByUserIdAsync(UserId.From(query.UserId));
        return profile;
    }

    public async Task<ProfileStatusReadModel?> Handle(GetProfileStatusQuery query)
    {
        var profile = await profileRepository.FindByUserIdAsync(UserId.From(query.UserId));
        if (profile is null) return null;

        return new ProfileStatusReadModel(
            ProfileId:            profile.ProfileId.Value,
            UserId:               profile.UserId.Value,
            Status:               profile.Status.ToString(),
            CompletionPercentage: CalculateCompletion(profile));
    }

    public async Task<bool> Handle(IsHomeownerActiveQuery query) => 
        await profileRepository.IsHomeownerActiveAsync(query.HomeownerId);
    
    private static int CalculateCompletion(Profile profile) =>
        profile.Status switch
        {
            EProfileStatus.Active => 100,
            EProfileStatus.Deactivated => 100,
            EProfileStatus.Incomplete => 50,
            EProfileStatus.Rejected => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(profile.Status), profile.Status, "Invalid profile status")
        };

    private static MyProfileReadModel MapToMyProfileReadModel(Profile profile)
    {
        return new MyProfileReadModel(
            ProfileId:    profile.ProfileId.Value,
            UserId:       profile.UserId.Value,
            BusinessRole: profile.BusinessRole?.ToString(),
            Status:       profile.Status.ToString(),
            PersonalData: profile.PersonalData is null ? null : MapPersonalData(profile.PersonalData),
            Technician:   profile.Technician   is null ? null : MapTechnician(profile.Technician),
            Homeowner:    profile.Homeowner    is null ? null : MapHomeowner(profile.Homeowner));
    }

    private static PersonalDataReadModel MapPersonalData(PersonalData personalData) =>
        new(personalData.FirstName, personalData.LastName,  personalData.PhoneNumber.Value,
            personalData.Address.ToString(), personalData.DateOfBirth.Value.ToString("yyyy-MM-dd"));

    private static TechnicianReadModel MapTechnician(Technician technician) =>
        new(technician.TechnicianId.Value,
            technician.Specialties.Select(s => s.ToString()).ToList(),
            technician.ExperienceYears, technician.AboutMe);

    private static HomeownerReadModel MapHomeowner(HomeOwner homeowner) =>
        new(homeowner.HomeownerId.Value,
            homeowner.PreferredContactTime.ToString(),
            homeowner.CommunicationPreferences.SmsNotifications,
            homeowner.CommunicationPreferences.EmailNotifications,
            homeowner.CommunicationPreferences.PushNotifications);
}
