using Hampcoders.Electrolink.API.IAM.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;

public class ExternalIamService(IIamContextFacade iamContextFacade)
{
    public async Task<bool> UserExistsAsync(int userId)
    {
        return await iamContextFacade.UserExistsAsync(userId);
    }
}