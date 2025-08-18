using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Transform;

/// <summary>
/// Assembler to create a CreateProfileCommand from a CreateProfileResource
/// </summary>
public static class CreateProfileCommandFromResourceAssembler
{
  /// <summary>
  /// Converts a CreateProfileResource to a CreateProfileCommand
  /// </summary>
  public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
  {
    var roleParsed = Enum.TryParse<Role>(resource.Role, ignoreCase: true, out var role)
      ? role
      : throw new ArgumentException($"Invalid role: {resource.Role}");

    return new CreateProfileCommand(0,
      resource.FirstName,
      resource.LastName,
      resource.Email,
      resource.Street,
      resource.Number,
      resource.City,
      resource.PostalCode,
      resource.Country,
      roleParsed,
      resource.Dni,
      resource.LicenseNumber,
      resource.Specialization
    );
  }
}

