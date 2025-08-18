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
    Task<Guid> Handle(AddPortfolioItemCommand command); // Nuevo
    Task<bool> Handle(UpdatePortfolioItemDetailsCommand command); // Nuevo
    Task<bool> Handle(RemovePortfolioItemCommand command); // Nuevo
}
