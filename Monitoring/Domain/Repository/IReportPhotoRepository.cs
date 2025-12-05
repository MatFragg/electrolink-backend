using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Repository;

public interface IReportPhotoRepository : IBaseRepository<ReportPhoto, Guid>
{
    Task<IEnumerable<ReportPhoto>> GetByReportIdAsync(Guid reportId);
    Task AddAsync(ReportPhoto reportPhoto);
}