using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Services;


public interface IServiceOperationQueryService
{
    Task<ServiceOperation?> Handle(GetServiceStatusByIdQuery query);
    Task<IEnumerable<ServiceOperation>> Handle(GetClientHistoryByTechnicianIdQuery query);
    
    Task<IEnumerable<ServiceOperation>> Handle(GetAllServiceOperationsQuery query);
}