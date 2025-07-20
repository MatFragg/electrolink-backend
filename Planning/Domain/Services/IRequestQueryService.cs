using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Services;

public interface IRequestQueryService
{
    Task<Request?> Handle(GetRequestDetailsQuery query);
    Task<IEnumerable<Request>> Handle(GetRequestsByClientIdQuery query);
}