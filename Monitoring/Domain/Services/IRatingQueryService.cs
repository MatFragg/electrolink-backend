using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Services;

public interface IRatingQueryService
{
    Task<Rating?> GetByRequestIdAsync(Guid requestId);
    Task<IEnumerable<Rating>> GetByTechnicianIdAsync(int technicianId);
    
    Task<IEnumerable<Rating>> Handle(GetAllRatingsQuery query);
}