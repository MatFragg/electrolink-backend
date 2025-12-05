using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Planning.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Planning.Application.ACL;

/// <summary>
/// Facade implementation for exposing read-only Request module operations to external bounded contexts.
/// </summary>
public class SdpContextFacade(IRequestQueryService requestQueryService) : ISDPContextFacade
{
    public async Task<Request?> FetchRequestDetailsAsync(string requestId)
    {
        var query = new GetRequestDetailsQuery(requestId);
        return await requestQueryService.Handle(query);
    }

    public async Task<IEnumerable<Request>> FetchRequestsByClientIdAsync(Guid clientId)
    {
        var query = new GetRequestsByClientIdQuery(clientId);
        return await requestQueryService.Handle(query);
    }
}