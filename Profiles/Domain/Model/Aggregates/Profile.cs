
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using InvalidOperationException = System.InvalidOperationException;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;

public partial class Profile : BaseAggregateRoot
{
    // ── Identity ──────────────────────────────────────────
    public ProfileId ProfileId { get; protected set; }
    public UserId UserId { get; private set; }

    // ── States ────────────────────────────────────────────
    public EProfileStatus Status { get; private set; }
    public EBusinessRole? BusinessRole { get; private set; }

    // ── Personal Data ─────
    public PersonalData? PersonalData { get; private set; }
    
    // ── Profile Picture ──
    public ProfilePhoto? Photo { get; private set; }

    // ── Sub-entities according to role ──
    public Technician? Technician { get; private set; }
    public HomeOwner? Homeowner { get; private set; }

    private Profile()
    {
    }

    public static Profile Create(UserId userId)
    {
        var profile = new Profile
        {
            ProfileId = ProfileId.NewProfileId(),
            UserId = userId,
            Status = EProfileStatus.Incomplete,
            BusinessRole = null,
            PersonalData = null,
            Technician = null,
            Homeowner = null,
        };

        profile.RaiseDomainEvent(new ProfileCreatedAsIncompleteEvent(
            profile.ProfileId, profile.UserId));

        return profile;
    }

    public async Task CompleteAsTechnician(
        PersonalData personalData,
        TechnicianData technicianData,
        IProfileUniquenessChecker uniquenessChecker
        )
    {
        EnsureStatus(EProfileStatus.Incomplete);
        await uniquenessChecker.EnsureDniIsUniqueAsync(personalData.Dni, ProfileId);

        PersonalData = personalData;
        BusinessRole = EBusinessRole.Technician;
        Technician = Technician.Create(TechnicianId.NewTechnicianId(), ProfileId, technicianData.Specialties, technicianData.ExperienceYears, technicianData.AboutMe, technicianData.ServiceArea);
        Status = EProfileStatus.Active;

        RaiseDomainEvent(new ProfileCompletedEvent(
            ProfileId.Value,
            UserId.Value,
            Technician!.TechnicianId.Value,
            EBusinessRole.Technician,
            DateTime.UtcNow));
    }

    public async Task CompleteAsHomeowner(
        PersonalData personalData,
        HomeownerData homeownerData,
        IProfileUniquenessChecker uniquenessChecker)
    {
        EnsureStatus(EProfileStatus.Incomplete);
        await uniquenessChecker.EnsureDniIsUniqueAsync(personalData.Dni, ProfileId);

        PersonalData = personalData;
        BusinessRole = EBusinessRole.HomeOwner;
        Homeowner = HomeOwner.Create(HomeownerId.NewHomeownerId(), ProfileId, homeownerData.PreferredContactTime, homeownerData.CommunicationPreferences, homeownerData.EmergencyContact);
        Status = EProfileStatus.Active;

        RaiseDomainEvent(new ProfileCompletedEvent(
            ProfileId.Value,
            UserId.Value,
            Homeowner!.HomeownerId.Value,
            EBusinessRole.HomeOwner,
            DateTime.UtcNow));
    }
    
    public void UpdateProfilePhoto(ProfilePhoto newPhoto)
    {
        if (newPhoto is null)
            throw new ArgumentNullException(nameof(newPhoto));

        Photo = newPhoto;
        RaiseDomainEvent(new ProfilePhotoUpdatedEvent(ProfileId, newPhoto.PublicUrl, newPhoto.ProviderId));
    }

    public string? RemoveProfilePhoto()
    {
        if (Photo is null)
            return null;

        var oldProviderId = Photo.ProviderId;
        Photo = null;
        RaiseDomainEvent(new ProfilePhotoRemovedEvent(ProfileId, oldProviderId));
        return oldProviderId;
    }

    public void UpdatePersonalData(string? firstName, string? lastName, PhoneNumber? phone, Address? address)
    {
        EnsureStatus(EProfileStatus.Active);
        PersonalData = PersonalData!.Update(firstName, lastName, phone, address);
        RaiseDomainEvent(new ProfilePersonalDataUpdatedEvent(
            ProfileId,
            firstName,
            lastName,
            phone?.Value));

    }

    public void UpdateTechnicianData(IEnumerable<ESpecialty>? specialties, int? experienceYears, string? aboutMe)
    {
        EnsureStatus(EProfileStatus.Active);
        EnsureRole(EBusinessRole.Technician);

        var specialtiesList = specialties?.ToList();

        if (specialtiesList is not null)
            Technician!.UpdateSpecialties(specialtiesList);

        if (experienceYears.HasValue)
            Technician!.UpdateExperienceYears(experienceYears.Value);
        
        if (aboutMe is not null)
            Technician!.UpdateAboutMe(aboutMe);

        RaiseDomainEvent(new TechnicianDataUpdatedEvent(
            ProfileId,
            specialtiesList,
            experienceYears,
            aboutMe,
            Technician!.TechnicianId));
    }
    
    public void UpdateHomeOwnerData(EContactTime? preferredContactTime, CommunicationPreferences? preferences, EmergencyContact? emergencyContact)
    {
        EnsureStatus(EProfileStatus.Active);
        EnsureRole(EBusinessRole.HomeOwner);

        // Solo aplicar cambios cuando se proporcionan (null => no cambiar)
        if (preferences != null)
            Homeowner!.UpdateCommunicationPreferences(preferences);

        if (preferredContactTime != null)
            Homeowner!.UpdatePreferredContactTime(preferredContactTime.Value);

        if (emergencyContact != null)
            Homeowner!.UpdateEmergencyContact(emergencyContact);

        RaiseDomainEvent(new HomeownerDataUpdatedEvent(
            ProfileId,
            Homeowner!.HomeownerId,
            preferredContactTime,
            preferences,
            emergencyContact));
    }

    public void Deactivate()
    {
        EnsureStatus(EProfileStatus.Active);
        
        Status = EProfileStatus.Deactivated;
        RaiseDomainEvent(new ProfileDeactivatedEvent(ProfileId, UserId, BusinessRole!.Value));
    }
    
    public void Reactivate()
    {
        EnsureStatus(EProfileStatus.Deactivated);
        Status = EProfileStatus.Active;
        RaiseDomainEvent(new ProfileReactivatedEvent(ProfileId, UserId, BusinessRole!.Value));
    }
    
    
    // ── Helper Methods for State Validation ────────────────────────────────
    private void EnsureStatus(EProfileStatus expected)
    {
        if (Status != expected)
            throw new InvalidProfileStatusException(ProfileId, expected, Status);
    }

    private void EnsureRole(EBusinessRole expected)
    {
        if (BusinessRole != expected)
            throw new InvalidBusinessRoleException(ProfileId, expected, BusinessRole);
    }
}

