using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record CreateProfileCommand(

  string FirstName,
  string LastName,
  string Email,
  string Street,
  string Number,
  string City,
  string PostalCode,
  string Country,
  Role Role, // <- HomeOwner o Technician

  // Campos exclusivos para HomeOwner
  string? PreferredContactTime,
  string? Dni,

  // Campos exclusivos para Technician
  string? LicenseNumber,
  string? Specialization
);
