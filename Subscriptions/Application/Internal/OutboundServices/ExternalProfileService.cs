using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;

/// <summary>
/// External profile service to interact with Profiles Bounded Context from Subscriptions Bounded Context.
/// </summary>
/// <param name="profilesContextFacade"></param>
/// <param name="logger"></param>
public class ExternalProfileService(IProfilesContextFacade profilesContextFacade, ILogger<ExternalProfileService> logger)
{
    /// <summary>
    /// Gets user email by profile ID.
    /// </summary>
    /// <returns>Email or empty string if not found</returns>
    public async Task<(string TechnicianId, string userId)?> FetchTechnicianInfoByProfileIdAsync(string profileId) 
        => await profilesContextFacade.GetTechnicianInfoByProfileIdAsync(profileId);

    
    /// <summary>
    /// Gets user full name by profile ID.
    /// </summary>
    /// <returns>Full name or empty string if not found</returns>
    public async Task<string> GetProfileFullNameAsync(string profileId)
        => await profilesContextFacade.GetProfileFullNameAsync(profileId);

    
    /// <summary>
    /// Gets user phone by profile ID.
    /// </summary>
    /// <returns>Phone or empty string if not found</returns>
    public async Task<string> GetProfilePhoneAsync(string profileId)
        => await profilesContextFacade.GetProfilePhoneAsync(profileId);
    
    
    /// <summary>
    /// Gets user role by profile ID.
    /// </summary>
    /// <returns>Role as string or empty if not found</returns>
    public async Task<string> GetProfileRoleAsync(string profileId) 
        => await profilesContextFacade.GetProfileRoleAsync(profileId);

    
    /// <summary>
    /// Checks if a profile exists for a given profile ID.
    /// </summary>
    public async Task<bool> ProfileExistsAsync(string profileId)
        => await profilesContextFacade.ProfileExistsAsync(profileId);
    
    /*
    /// <summary>
    /// Gets technician ID (as string) by profile ID.
    /// </summary>
    /// <returns>Technician GUID as string, or null if not found</returns>
    public async Task<Guid?> GetTechnicianIdAsync(int profileId)
    
        => await profilesContextFacade.GetTechnicianIdByProfileIdAsync(profileId);
    */
    
    /// <summary>
    /// Gets technician information (TechnicianId and UserId).
    /// </summary>
    /// <returns>Tuple with (technicianId as string, userId as int), or null if not found</returns>
    public async Task<(string technicianId, string userId)?> GetTechnicianInfoAsync(string profileId)
        => await profilesContextFacade.GetTechnicianInfoByProfileIdAsync(profileId);
    

    /// <summary>
    /// Checks if a technician profile exists for a given user ID.
    /// </summary>
    public async Task<bool> IsTechnicianAsync(string profileId) 
        => await profilesContextFacade.ExistsTechnicianProfileByUserIdAsync(profileId);
    
    public async Task<string> FetchProfileEmail(string userId) 
        => await profilesContextFacade.GetProfileEmailAsync(userId);
    
    
    public async Task<string> FetchProfileFullName(string userId)
        => await profilesContextFacade.GetProfileFullNameAsync(userId);
    
}