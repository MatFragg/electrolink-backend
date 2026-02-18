using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Domain.Repositories;

public interface IRequestRepository : IBaseRepository<Request, RequestId>
{
    Task<IEnumerable<Request>> ListByClientIdAsync(ClientId clientId);
    Task<int> CountByClientIdAndMonthAsync(ClientId clientId, int year, int month);
    Task UpdateAsync(Request request);
    Task DeleteAsync(Request request);
}