using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;

public class ServiceOperationQueryService(
    IServiceOperationRepository repo
) : IServiceOperationQueryService
{
    public async Task<ServiceOperation?> Handle(GetServiceStatusByIdQuery query)
    {
        return await repo.FindByIdAsync(query.RequestId);
    }
    
    public async Task<IEnumerable<ServiceOperation>> Handle(GetClientHistoryByTechnicianIdQuery query)
    {
        return await repo.GetByTechnicianIdAsync(query.TechnicianId);
    }
    
    public async Task<IEnumerable<ServiceOperation>> Handle(GetAllServiceOperationsQuery query) =>
        await repo.ListAsync();
}