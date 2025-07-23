using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Queries;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Application.ACL;

/// <summary>
/// Facade implementation for exposing read-only Request module operations to external bounded contexts.
/// </summary>
public class SDPContextFacade(IRequestQueryService requestQueryService) : ISDPContextFacade
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