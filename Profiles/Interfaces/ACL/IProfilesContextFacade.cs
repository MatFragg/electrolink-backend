namespace Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;


/// <summary>
/// Facade for the profiles context
/// </summary>
public interface IProfilesContextFacade
{
    /// <summary>
    /// Create a complete profile (Homeowner or Technician)
    /// </summary>
    Task<string> CreateProfile(
        string userId);
    
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

}


