using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IRequestQueryService
{
    Task<Request?> Handle(GetRequestDetailsQuery query);
    Task<IEnumerable<Request>> Handle(GetRequestsByClientIdQuery query);
}