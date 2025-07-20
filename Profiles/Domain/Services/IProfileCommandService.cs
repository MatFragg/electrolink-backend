using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Services;

/// <summary>
/// Command service interface for creating profiles (Homeowners or Technicians).
/// </summary>
public interface IProfileCommandService
{
    Task<Profile?> Handle(CreateProfileCommand command);
}
