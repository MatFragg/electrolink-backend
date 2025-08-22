namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.ACL;

public interface ISubscriptionContextFacade
{
    Task<bool> RecordServiceRequestUsageAsync(int ownerUserId);
}