using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Repository;

public interface IServiceOperationRepository : IBaseRepository<ServiceOperation>
{
    Task<IEnumerable<ServiceOperation>> GetByTechnicianIdAsync(int technicianId);
    
}