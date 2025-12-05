using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;

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