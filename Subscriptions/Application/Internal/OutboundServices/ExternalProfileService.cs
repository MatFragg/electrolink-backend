using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;

public class ExternalProfileService(IProfilesContextFacade profilesContextFacade, ILogger<ExternalProfileService> logger)
{
    public async Task<(string TechnicianId, string UserId)?> GetTechnicianInfoByProfileIdAsync(string profileId)
        => await profilesContextFacade.GetTechnicianInfoByProfileIdAsync(profileId);

    public async Task<string?> GetProfileFullNameAsync(string profileId)
        => await profilesContextFacade.GetProfileFullNameAsync(profileId);

    public async Task<string?> FetchProfileFullName(string userId)
    {
        var claims = await profilesContextFacade.GetProfileClaimsAsync(userId);
        if (claims is null) return null;
        return await profilesContextFacade.GetProfileFullNameAsync(claims.Value.ProfileId);
    }

    public async Task<string?> GetProfilePhoneAsync(string profileId)
        => await profilesContextFacade.GetProfilePhoneAsync(profileId);

    public async Task<string?> GetProfileRoleAsync(string profileId)
        => await profilesContextFacade.GetProfileRoleAsync(profileId);

    public async Task<bool> ProfileExistsAsync(string profileId)
        => await profilesContextFacade.ProfileExistsAsync(profileId);

    public async Task<bool> IsTechnicianAsync(string profileId)
        => await profilesContextFacade.ExistsTechnicianProfileByUserIdAsync(profileId);
}