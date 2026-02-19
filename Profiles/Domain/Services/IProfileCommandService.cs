using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Services;

/// <summary>
/// Command service interface for creating profiles (Homeowners or Technicians).
/// </summary>

public interface IProfileCommandService
{
    Task<Profile?> Handle(CreateProfileCommand command);
    Task<bool> Handle(UpdateProfileCommand command);
    Task<bool> Handle(AssignHomeOwnerInfoCommand command);
    Task<bool> Handle(AssignTechnicianInfoCommand command);
    Task<bool> Handle(UpdateTechnicianCoverageCommand command);
    Task<bool> Handle(UpdateTechnicianSpecialtiesCommand command);
    Task<Guid> Handle(AddPortfolioItemCommand command); 
    Task<bool> Handle(UpdatePortfolioItemDetailsCommand command);
    Task<bool> Handle(RemovePortfolioItemCommand command); 
    
    Task<Profile> Handle(CompleteProfileAsTechnicianCommand command);
    Task<Profile> Handle(CompleteProfileAsHomeownerCommand command);
    Task<Profile> Handle(UpdateProfilePersonalDataCommand command);
    Task<Profile> Handle(UpdateTechnicianDataCommand command);
    Task<Profile> Handle(AddCertificationCommand command);
    Task<Profile> Handle(UpdateCertificationCommand command);
    Task<Profile> Handle(UpdateCommunicationPreferencesCommand command);
    Task Handle(DeactivateProfileCommand command);
    Task Handle(ReactivateProfileCommand command);
}
