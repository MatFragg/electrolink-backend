using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.ACL;

/// <summary>
/// Facade interface for exposing read-only operations of the Request module.
/// </summary>
public interface ISDPContextFacade
{
    /// <summary>
    /// Get a request by its ID.
    /// </summary>
    /// <param name="requestId">The ID of the request to retrieve.</param>
    /// <returns>The request if found; otherwise, null.</returns>
    Task<Request?> FetchRequestDetailsAsync(string requestId);

    /// <summary>
    /// Get all requests associated with a specific client ID.
    /// </summary>
    /// <param name="clientId">The ID of the client.</param>
    /// <returns>A list of requests belonging to the client.</returns>
    Task<IEnumerable<Request>> FetchRequestsByClientIdAsync(Guid clientId);
}