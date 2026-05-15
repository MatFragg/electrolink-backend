// ISubscriptionsContextFacade.cs
// Puerto que Planning BC consume para verificar elegibilidad de solicitudes.

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.ACL;

public interface ISubscriptionsContextFacade
{
    Task<bool> IsUserPremiumAsync(string userId);
    Task<RequestEligibilityResult> GetRequestEligibilityAsync(string userId);
}
