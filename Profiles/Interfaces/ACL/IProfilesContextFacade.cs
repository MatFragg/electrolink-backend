namespace Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;


/// <summary>
/// Facade for the profiles context
/// </summary>
public interface IProfilesContextFacade
{
    /// <summary>
    /// Gets technician ID (Guid as string) by profile ID.
    /// </summary>
    /// <returns>Technician GUID as string, or null if not found</returns>
    Task<string?> GetTechnicianIdByUserIdAsync(string userId);
    
    /// <summary>
    /// Gets technician information (TechnicianId and UserId).
    /// </summary>
    /// <returns>Tuple with (technicianId as string, userId as int), or null if not found</returns>
    Task<(string technicianId, string userId)?> GetTechnicianInfoByProfileIdAsync(string userId);
    
    /// <summary>
    /// Checks if a technician profile exists for a given user ID.
    /// </summary>
    Task<bool> ExistsTechnicianProfileByUserIdAsync(string userId);

    /// <summary>
    /// Gets profile full name by profile ID.
    /// </summary>
    /// <returns>Full name or empty string if not found</returns>
    Task<string> GetProfileFullNameAsync(string profileId);

    /// <summary>
    /// Gets profile phone by profile ID.
    /// </summary>
    /// <returns>Phone or empty string if not found</returns>
    Task<string> GetProfilePhoneAsync(string profileId);

    /// <summary>
    /// Gets profile role by profile ID.
    /// </summary>
    /// <returns>Role as string ("HomeOwner" or "Technician") or empty if not found</returns>
    Task<string> GetProfileRoleAsync(string profileId);

    /// <summary>
    /// Checks if a profile exists for a given profile ID.
    /// </summary>
    Task<bool> ProfileExistsAsync(string profileId);

    /// <summary>
    /// Checks if a homeowner profile is active.
    /// </summary>
    Task<bool> IsHomeownerActiveAsync(string homeownerId);

    /// <summary>
    /// Checks if a homeowner has at least one registered property.
    /// </summary>
    Task<bool> HomeownerHasPropertiesAsync(string homeownerId);

    /// <summary>
    /// Gets all technicians whose service area contains the given coordinates.
    /// Returns a list of tuples with (technicianId, profileId, fullName, rating).
    /// Specialties are excluded to keep primitives — query separately if needed.
    /// </summary>
    Task<IEnumerable<(string technicianId, string profileId, string fullName, double rating)>> GetTechniciansInAreaAsync(double latitude, double longitude);
    
    /// <summary>
    /// Gets specialties for a given technician.
    /// </summary>
    Task<IEnumerable<string>> GetTechnicianSpecialtiesAsync(string technicianId);
    
    /// <summary>
    /// Gets profile claims (full name, role, and profile ID) by user ID.
    /// </summary>
    Task<(string ProfileId, string ProfileStatus, string? BusinessRole, string? RoleSubjectId)?> GetProfileClaimsAsync(string userId);
}


