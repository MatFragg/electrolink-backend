using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Queries;
using Hamcoders.Electrolink.API.Monitoring.Domain.Repository;
using Hamcoders.Electrolink.API.Monitoring.Domain.Services;

namespace Hamcoders.Electrolink.API.Monitoring.Application.Internal.QueryServices;

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