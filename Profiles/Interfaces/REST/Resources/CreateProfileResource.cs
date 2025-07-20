namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;



/// <summary>
/// Resource for creating a new profile (Homeowner or Technician)
/// </summary>
public record CreateProfileResource(
  string FirstName,
  string LastName,
  string Email,
  string Street,
  string Number,
  string City,
  string PostalCode,
  string Country,
  string Role, // "HomeOwner" o "Technician"

  // Campos exclusivos para HomeOwner
  string? PreferredContactTime,
  string? Dni,

  // Campos exclusivos para Technician
  string? LicenseNumber,
  string? Specialization
);
