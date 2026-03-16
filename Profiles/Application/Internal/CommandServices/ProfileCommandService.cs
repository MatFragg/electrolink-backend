using Hampcoders.Electrolink.API.Profiles.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.CommandServices;

public class ProfileCommandService(
  IProfileRepository profileRepository,
  ExternalAssetService externalAssetService,
  ExternalIamService externalIamService,
  IUnitOfWork unitOfWork,
  IMediator mediator,
  IProfileUniquenessChecker uniquenessChecker,
  ILogger<ProfileCommandService> logger)
  : IProfileCommandService
{
  public async Task<Profile?> Handle(CreateProfileCommand command)
  { 
      var userId = UserId.From(command.UserId);
      
      if (await profileRepository.ExistsByUserIdAsync(userId))
      {
          logger.LogWarning("[Profiles BC] Perfil ya existe para {UserId}. Evento duplicado ignorado.", command.UserId);
          return await profileRepository.FindByUserIdAsync(userId);
      }
      
      var profile = Profile.Create(userId);
      await profileRepository.AddAsync(profile);
      await unitOfWork.CompleteAsync();
      
      return profile;
  }

  public Task<bool> Handle(UpdateTechnicianSpecialtiesCommand command)
  {
      throw new NotImplementedException();
  }

  public async Task<Profile> Handle(CompleteProfileAsTechnicianCommand command)
  {
      var profile = await profileRepository.FindByUserIdAsync(
          UserId.From(command.UserId));
      
      if (profile is null)
          throw new ArgumentException("Profile not found.");
      
      var personalData = PersonalData.Create(
          command.FirstName, 
          command.LastName, 
          PhoneNumber.From(command.PhoneNumber), 
          Dni.From(command.Dni),
          DateOfBirth.From(command.DateOfBirth), 
          Address.Create(command.Street, command.Number, command.District, command.City, command.Country, command.PostalCode));
      
      var technicianData = TechnicianData.Create(
          command.Specialties, 
          command.ExperienceYears,
          command.AboutMe);
      
      profile.CompleteAsTechnician(personalData, technicianData, uniquenessChecker);

      profileRepository.Update(profile);
      await unitOfWork.CompleteAsync();
      await PublishAndClearEventsAsync(profile);
      return profile;
  }

  public async Task<Profile> Handle(CompleteProfileAsHomeownerCommand command)
  {
      var profile = await profileRepository.FindByUserIdAsync(
          UserId.From(command.UserId));

      if (profile is null)
          throw new ArgumentException("Profile not found.");

      var personalData = PersonalData.Create(
          command.FirstName,
          command.LastName,
          PhoneNumber.From(command.PhoneNumber),
          Dni.From(command.Dni),
          DateOfBirth.From(command.DateOfBirth),
          Address.Create(command.Street, command.Number, command.District, command.City, command.Country, command.PostalCode)
      );

      var homeownerData = HomeownerData.Create(
          command.PreferredContactTime,
          command.CommunicationPreferences,
          command.EmergencyContact
      );

      profile.CompleteAsHomeowner(personalData, homeownerData, uniquenessChecker);

      profileRepository.Update(profile);
      await unitOfWork.CompleteAsync();
      await PublishAndClearEventsAsync(profile);
      return profile;
  }

  public async Task<Profile> Handle(UpdateProfilePersonalDataCommand command)
  {
      var profile = await profileRepository.FindByIdAsync(
          ProfileId.From(command.ProfileId)) ?? throw new ArgumentException("Profile not found.");
      
      EnsureOwnership(profile, command.UserId);

      Address? address = null;
      
      if (command.Street is not null || command.District is not null ||
          command.City   is not null || command.Country  is not null ||
          command.PostalCode is not null)
      {
          var current = profile.PersonalData!.Address;
          address = Address.Create(
              command.Street ?? current.Street, 
              command.Number ?? current.Number,
              command.District ?? current.District,
              command.City ?? current.City,
              command.Country ?? current.Country,
              command.PostalCode ?? current.PostalCode);
      }
      
      var phone = command.PhoneNumber is not null
          ? PhoneNumber.From(command.PhoneNumber)
          : null;

      profile.UpdatePersonalData(command.FirstName, command.LastName, phone, address);

      profileRepository.Update(profile);
      await unitOfWork.CompleteAsync();
      return profile;
  }

  public async Task<Profile> Handle(UpdateTechnicianDataCommand command)
  {
      var profile = await profileRepository.FindByIdAsync(
          ProfileId.From(command.ProfileId)) ?? throw new ArgumentException("Profile not found.");
      
      EnsureOwnership(profile, command.UserId);

      profile.UpdateTechnicianData(
          command.Specialties,
          command.ExperienceYears, 
          command.AboutMe);
      
      profileRepository.Update(profile);
      await unitOfWork.CompleteAsync();
      return profile;
  }

  public async Task<Profile> Handle(UpdateCommunicationPreferencesCommand command)
  {
      var profile = await profileRepository.FindByIdAsync(
          ProfileId.From(command.ProfileId)) ?? throw new ArgumentException("Profile not found.");
      
      EnsureOwnership(profile, command.UserId);

      var current = profile.Homeowner!.CommunicationPreferences;
      var preferences = CommunicationPreferences.Create(
          command.SmsNotifications   ?? current.SmsNotifications,
          command.EmailNotifications ?? current.EmailNotifications,
          command.PushNotifications  ?? current.PushNotifications,
          command.PreferredContactTime ?? profile.Homeowner!.PreferredContactTime);

      var emergencyContact = command.EmergencyContact;
      
      profile.UpdateHomeOwnerData(
          command.PreferredContactTime,
          preferences,
          emergencyContact);
      
      profileRepository.Update(profile);
      await unitOfWork.CompleteAsync();

      return profile;
  }

  public async Task Handle(DeactivateProfileCommand command)
  {
      var profile = await profileRepository.FindByIdAsync(
          ProfileId.From(command.ProfileId)) ?? throw new ArgumentException("Profile not found.");
      
      EnsureOwnership(profile, command.UserId);
      
      profile.Deactivate();
      profileRepository.Update(profile);
      await unitOfWork.CompleteAsync();
  }

  public async Task Handle(ReactivateProfileCommand command)
  {
      var profile = await profileRepository.FindByIdAsync(
          ProfileId.From(command.ProfileId)) ?? throw new ArgumentException("Profile not found.");

      EnsureOwnership(profile, command.UserId);

      profile.Reactivate();

      profileRepository.Update(profile);
      await unitOfWork.CompleteAsync();
      await PublishAndClearEventsAsync(profile);
  }
  
  private async Task PublishAndClearEventsAsync(Profile profile)
  {
      foreach (var domainEvent in profile.DomainEvents)
          await mediator.Publish(domainEvent, CancellationToken.None);
      profile.ClearDomainEvents();
  }
  
  private static void EnsureOwnership(Profile profile, string userId)
  {
      if (profile.UserId != UserId.From(userId))
          throw new UnauthorizedProfileAccessException();
  }
}

