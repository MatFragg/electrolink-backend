using Hampcoders.Electrolink.API.Profiles.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Domain.Repositories;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.CommandServices;

public class ProfileCommandService(
  IProfileRepository profileRepository,
  ExternalAssetService externalAssetService,
  IUnitOfWork unitOfWork)
  : IProfileCommandService
{
  public async Task<Profile?> Handle(CreateProfileCommand command)
  {
    var profile = new Profile(
      command.FirstName,
      command.LastName,
      command.Email,
      command.Street,
      command.Number,
      command.City,
      command.PostalCode,
      command.Country,
      command.Role
    );

    try
    {
      if (command.Role == Role.HomeOwner)
      {
        profile.AssignHomeOwnerInfo(command.Dni ?? string.Empty);
      }
      else if (command.Role == Role.Technician)
      {
        profile.AssignTechnicianInfo(
          command.LicenseNumber ?? string.Empty,
          command.Specialization ?? string.Empty
        );
      }

      await profileRepository.AddAsync(profile);
      
      if (command.Role == Role.Technician && profile.Technician != null)
      {
        await externalAssetService.CreateTechnicianInventoryAsync(profile.Technician.Id);
      }
      
      await unitOfWork.CompleteAsync();
      return profile;
    }
    catch (Exception)
    {
      return null;
    }
  }
}

