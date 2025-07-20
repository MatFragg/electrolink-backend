using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Services;

public interface IRatingQueryService
{
    Task<Rating?> GetByRequestIdAsync(Guid requestId);
    Task<IEnumerable<Rating>> GetByTechnicianIdAsync(int technicianId);
    
    Task<IEnumerable<Rating>> Handle(GetAllRatingsQuery query);
}