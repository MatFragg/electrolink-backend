using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Repository;

public interface IRatingRepository: IBaseRepository<Rating>
{
    Task<Rating?> GetByRequestIdAsync(Guid requestId);
    
    Task<IEnumerable<Rating>> GetByTechnicianIdAsync(int technicianId);
    
}