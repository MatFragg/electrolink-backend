using Hampcoders.Electrolink.API.Profiles.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.CommandServices;

public class ProfileCommandService(
  IProfileRepository profileRepository,
  ExternalAssetService externalAssetService,
  ExternalIamService externalIamService,
  IUnitOfWork unitOfWork,
  IMediator mediator,
  ILogger<ProfileCommandService> logger)
  : IProfileCommandService
{
  public async Task<Profile?> Handle(CreateProfileCommand command)
  { 
      
      if (!await externalIamService.UserExistsAsync(command.UserId))
      {
          logger.LogWarning("[ProfileCommandService] Failed to create profile: IAM User ID {UserId} does not exist in IAM Bounded Context.", command.UserId);
          throw new ArgumentException($"User with ID {command.UserId} does not exist in Identity and Access Management.");
      }
      
      var emailAddress = new EmailAddress(command.Email); 
      if (await profileRepository.ExistsByEmailAsync(emailAddress.Address)) 
      {
          logger.LogWarning("[ProfileCommandService] Attempt to create profile with duplicate email: {Email}", command.Email);
          throw new InvalidOperationException($"A profile with the email {command.Email} already exists.");
      } 
      
      if (await profileRepository.FindByProfileIdAsync(command.UserId) != null) 
      {
          logger.LogWarning("[ProfileCommandService] Attempt to create profile for already existing IAM User ID (Profile ID): {UserId}", command.UserId);
          throw new InvalidOperationException($"User already exists.");
      }
      
      var profile = new Profile(command.UserId, command); 
      await profileRepository.AddAsync(profile); 
      await unitOfWork.CompleteAsync(); 
      
      profile.SetIdAfterPersistence(profile.Id);
      
      if (profile.Role == Role.Technician && profile.Technician != null)
      {
          await externalAssetService.CreateTechnicianInventoryAsync(profile.Technician.Id);
          logger.LogInformation("[ProfileCommandService] Created inventory for new technician: {TechnicianId}", profile.Technician.Id);
      }
      
      foreach (var domainEvent in profile.DomainEvents)
      {
          logger.LogInformation("[ProfileCommandService] Publishing domain event: {EventType} (ID: {EventId})", 
              domainEvent.GetType().Name, domainEvent.EventId);
          await mediator.Publish(domainEvent, CancellationToken.None);
      }
      profile.ClearDomainEvents();
    
      logger.LogInformation("[ProfileCommandService] Successfully created profile for email: {Email} with ID: {ProfileId}", 
          command.Email, profile.Id);
      return profile;
  }
  public async Task<bool> Handle(UpdateProfileCommand command)
  {
      var profile = await profileRepository.FindByProfileIdAsync(command.ProfileId);
      if (profile is null) throw new ArgumentException("Profile not found.");
      profile.UpdateProfileInfo(command.FirstName, command.LastName, command.Email, command.Street, command.Number, command.City, command.PostalCode, command.Country);
      await unitOfWork.CompleteAsync();
      foreach (var domainEvent in profile.DomainEvents) { await mediator.Publish(domainEvent, CancellationToken.None); }
      profile.ClearDomainEvents();
      return true;
  }

  public async Task<bool> Handle(AssignHomeOwnerInfoCommand command)
  {
      var profile = await profileRepository.FindByProfileIdAsync(command.ProfileId);
      if (profile is null) throw new ArgumentException("Profile not found.");
      if (profile.Role != Role.HomeOwner) throw new InvalidOperationException("Profile is not a HomeOwner.");
      profile.AssignHomeOwnerInfo(command.Dni);
      await unitOfWork.CompleteAsync();
      foreach (var domainEvent in profile.DomainEvents) { await mediator.Publish(domainEvent, CancellationToken.None); }
      profile.ClearDomainEvents();
      return true;
  }

  public async Task<bool> Handle(AssignTechnicianInfoCommand command)
  {
      var profile = await profileRepository.FindByProfileIdAsync(command.ProfileId);
      if (profile is null) throw new ArgumentException("Profile not found.");
      if (profile.Role != Role.Technician) throw new InvalidOperationException("Profile is not a Technician.");
      profile.AssignTechnicianInfo(command.LicenseNumber, command.Specialization);
      await unitOfWork.CompleteAsync();
      foreach (var domainEvent in profile.DomainEvents) { await mediator.Publish(domainEvent, CancellationToken.None); }
      profile.ClearDomainEvents();
      return true;
  }

  public async Task<bool> Handle(UpdateTechnicianCoverageCommand command)
  {
      var profile = await profileRepository.FindByProfileIdAsync(command.ProfileId);
      if (profile is null) throw new ArgumentException("Profile not found.");
      if (profile.Role != Role.Technician) throw new InvalidOperationException("Profile is not a Technician.");
      profile.UpdateTechnicianCoverageArea(command.NewCoverageAreaDetails);
      await unitOfWork.CompleteAsync();
      foreach (var domainEvent in profile.DomainEvents) { await mediator.Publish(domainEvent, CancellationToken.None); }
      profile.ClearDomainEvents();
      return true;
  }

  public async Task<bool> Handle(UpdateTechnicianSpecialtiesCommand command)
  {
      var profile = await profileRepository.FindByProfileIdAsync(command.ProfileId);
      if (profile is null) throw new ArgumentException("Profile not found.");
      if (profile.Role != Role.Technician) throw new InvalidOperationException("Profile is not a Technician.");
      profile.UpdateTechnicianSpecialties(command.NewSpecialties);
      await unitOfWork.CompleteAsync();
      foreach (var domainEvent in profile.DomainEvents) { await mediator.Publish(domainEvent, CancellationToken.None); }
      profile.ClearDomainEvents();
      return true;
  }
  public async Task<Guid> Handle(AddPortfolioItemCommand command)
  {
    var profile = await profileRepository.FindByProfileIdAsync(command.ProfileId); 
    if (profile is null) throw new ArgumentException("Profile not found.");
    var newPortfolioItem = profile.AddPortfolioItemToTechnician(command.Title, command.Description, command.ImageUrl);
    await unitOfWork.CompleteAsync();
    foreach (var domainEvent in profile.DomainEvents) { await mediator.Publish(domainEvent, CancellationToken.None); }
    profile.ClearDomainEvents();
    return newPortfolioItem.WorkId;
  }

  public async Task<bool> Handle(UpdatePortfolioItemDetailsCommand command)
  {
      var profile = await profileRepository.FindByProfileIdAsync(command.ProfileId);
      if (profile is null) throw new ArgumentException("Profile not found.");
      profile.UpdateTechnicianPortfolioItemDetails(command.WorkId, command.NewTitle, command.NewDescription, command.NewImageUrl);
      await unitOfWork.CompleteAsync();
      foreach (var domainEvent in profile.DomainEvents) { await mediator.Publish(domainEvent, CancellationToken.None); }
      profile.ClearDomainEvents();
      return true;
  }

  public async Task<bool> Handle(RemovePortfolioItemCommand command)
  {
      var profile = await profileRepository.FindByProfileIdAsync(command.ProfileId);
      if (profile is null) throw new ArgumentException("Profile not found.");
      profile.RemoveTechnicianPortfolioItem(command.WorkId);
      await unitOfWork.CompleteAsync();
      foreach (var domainEvent in profile.DomainEvents) { await mediator.Publish(domainEvent, CancellationToken.None); }
      profile.ClearDomainEvents();
      return true;
  }
}

