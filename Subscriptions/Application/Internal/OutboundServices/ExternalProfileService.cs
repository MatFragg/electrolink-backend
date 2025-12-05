using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;

public class ExternalProfileService(IProfilesContextFacade profilesContextFacade)
{
    public async Task<(Guid TechnicianId, int userId)?> FetchTechnicianInfoByProfileIdAsync(int profileId)
    {
        return await profilesContextFacade.GetTechnicianInfoByProfileIdAsync(profileId);
    }
    
}