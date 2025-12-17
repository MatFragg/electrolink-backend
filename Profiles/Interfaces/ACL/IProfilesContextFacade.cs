namespace Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;


/// <summary>
/// Facade for the profiles context
/// </summary>
public interface IProfilesContextFacade
{
    /// <summary>
    /// Create a complete profile (Homeowner or Technician)
    /// </summary>
    Task<int> CreateProfile(
        int userId,
        string firstName,
        string lastName,
        string email,
        string street,
        string number,
        string city,
        string postalCode,
        string country,
        string role,
        string? dni,
        string? licenseNumber,
        string? specialization);

    /// <summary>
    /// Fetch the profile id by email   
    /// </summary>
    Task<int> FetchProfileIdByEmail(string email);
    
    /// <summary>
    /// Gets technician ID (Guid as string) by profile ID.
    /// </summary>
    /// <returns>Technician GUID as string, or null if not found</returns>
    Task<Guid?> GetTechnicianIdByProfileIdAsync(int profileId);
    
    /// <summary>
    /// Gets technician information (TechnicianId and UserId).
    /// </summary>
    /// <returns>Tuple with (technicianId as string, userId as int), or null if not found</returns>
    Task<(Guid technicianId, int userId)?> GetTechnicianInfoByProfileIdAsync(int profileId);
    
    /// <summary>
    /// Checks if a technician profile exists for a given user ID.
    /// </summary>
    Task<bool> ExistsTechnicianProfileByUserIdAsync(int userId);
    
    /// <summary>
    /// Gets profile email by profile ID.
    /// </summary>
    /// <returns>Email or empty string if not found</returns>
    Task<string> GetProfileEmailAsync(int profileId);

    /// <summary>
    /// Gets profile full name by profile ID.
    /// </summary>
    /// <returns>Full name or empty string if not found</returns>
    Task<string> GetProfileFullNameAsync(int profileId);

    /// <summary>
    /// Gets profile phone by profile ID.
    /// </summary>
    /// <returns>Phone or empty string if not found</returns>
    Task<string> GetProfilePhoneAsync(int profileId);

    /// <summary>
    /// Gets profile role by profile ID.
    /// </summary>
    /// <returns>Role as string ("HomeOwner" or "Technician") or empty if not found</returns>
    Task<string> GetProfileRoleAsync(int profileId);

    /// <summary>
    /// Checks if a profile exists for a given profile ID.
    /// </summary>
    Task<bool> ProfileExistsAsync(int profileId);

}


