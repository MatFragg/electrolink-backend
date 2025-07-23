using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;

public interface IRequestQueryService
{
    Task<Request?> Handle(GetRequestDetailsQuery query);
    Task<IEnumerable<Request>> Handle(GetRequestsByClientIdQuery query);
}