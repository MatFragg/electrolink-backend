using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Repositories;

public interface IRequestRepository : IBaseRepository<Request>
{
    Task<IEnumerable<Request>> ListByClientIdAsync(Guid clientId);
    Task<Request?> FindByIdAsync(string requestId);
    Task UpdateAsync(Request request);
    Task DeleteAsync(Request request);
}