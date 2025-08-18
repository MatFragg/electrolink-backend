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
    
    Task<Guid?> GetTechnicianIdByProfileIdAsync(int profileId);
    
    Task<(Guid technicianId, int userId)?> GetTechnicianInfoByProfileIdAsync(int profileId);
    
    Task<bool> ExistsTechnicianProfileByUserIdAsync(int userId);
}


