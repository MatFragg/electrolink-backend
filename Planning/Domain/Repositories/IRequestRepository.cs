using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IRequestRepository : IBaseRepository<Request, string>
{
    Task<IEnumerable<Request>> ListByClientIdAsync(Guid clientId);
    Task UpdateAsync(Request request);
    Task DeleteAsync(Request request);
}