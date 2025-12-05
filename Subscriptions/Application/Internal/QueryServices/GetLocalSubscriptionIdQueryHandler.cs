using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class GetLocalSubscriptionIdQueryHandler 
    : IRequestHandler<GetLocalSubscriptionIdQuery, Guid?>
{
    private readonly ISubscriptionQueryService _queryService;

    public GetLocalSubscriptionIdQueryHandler(ISubscriptionQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<Guid?> Handle(GetLocalSubscriptionIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.Handle(query);
    }
}
