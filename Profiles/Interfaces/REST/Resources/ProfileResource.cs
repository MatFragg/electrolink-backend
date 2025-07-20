namespace Hampcoders.Electrolink.API.Profiles.Interfaces.REST.Resources;


/// <summary>
/// Resource returned when retrieving a profile
/// </summary>
public record ProfileResource(
  int Id,
  string FullName,
  string Email,
  string StreetAddress,
  string Role, // "HomeOwner" o "Technician"


  // Campos opcionales dependiendo del rol
  string? Dni,
  string? LicenseNumber,
  string? Specialization,
  Guid? HomeOwnerId, // Solo si es HomeOwner
  Guid? TechnicianId // Solo si es Technician
);
