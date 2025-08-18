using Hampcoders.Electrolink.API.Profiles.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record CreateProfileCommand(
  int UserId,
  string FirstName,
  string LastName,
  string Email,
  string Street,
  string Number,
  string City,
  string PostalCode,
  string Country,
  Role Role, 
  string? Dni,
  string? LicenseNumber,
  string? Specialization
);
