using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.API.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.API.Application.Internal.QueryServices;

public class RequestQueryService(IRequestRepository requestRepository) : IRequestQueryService
{
    public async Task<Request?> Handle(GetRequestDetailsQuery query)
    {
        return await requestRepository.FindByIdAsync(query.RequestId);
    }

    public async Task<IEnumerable<Request>> Handle(GetRequestsByClientIdQuery query)
    {
        return await requestRepository.ListByClientIdAsync(query.ClientId);
    }
}