using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Monitoring.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Services;

public interface IReportQueryService
{
    Task<Report?> GetByRequestIdAsync(Guid requestId);
    
    Task<IEnumerable<Report>> Handle(GetAllReportsQuery query);
    
    Task<Report?> GetByIdWithPhotosAsync(Guid reportId);
}

