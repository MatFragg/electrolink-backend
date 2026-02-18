using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;

public class RequestQueryService(IRequestRepository requestRepository) : IRequestQueryService
{
    public async Task<Request?> Handle(GetRequestDetailsQuery query)
    {
        var requestId = new RequestId(query.RequestId);
        return await requestRepository.FindByIdAsync(requestId);
    }

    public async Task<IEnumerable<Request>> Handle(GetRequestsByClientIdQuery query)
    {
        var clientId = new ClientId(query.ClientId);
        return await requestRepository.ListByClientIdAsync(clientId);
    }
}