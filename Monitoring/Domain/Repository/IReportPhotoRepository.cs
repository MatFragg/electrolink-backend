using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Repository;

public interface IReportPhotoRepository : IBaseRepository<ReportPhoto>
{
    Task<IEnumerable<ReportPhoto>> GetByReportIdAsync(Guid reportId);
    Task AddAsync(ReportPhoto reportPhoto);
}