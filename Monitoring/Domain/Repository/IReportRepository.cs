using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Repository;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<Report?> GetByRequestIdAsync(Guid requestId);
    
}