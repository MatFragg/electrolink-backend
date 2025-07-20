using Hampcoders.Electrolink.API.Profiles.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

/// <summary>
/// Assembler to convert Profile entity to ProfileResource
/// </summary>
public static class ProfileResourceFromEntityAssembler
{
  public static ProfileResource ToResourceFromEntity(Profile entity)
  {
    return new ProfileResource(
      entity.Id,
      entity.FullName,
      entity.EmailAddress,
      entity.StreetAddress,
      entity.Role.ToString(),
      entity.HomeOwner?.Dni,
      entity.Technician?.LicenseNumber,
      entity.Technician?.Specialization,
      entity.HomeOwner?.Id, // Solo si es HomeOwner
      entity.Technician?.Id // Solo si es Technician
    );
  }
}
