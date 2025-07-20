using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Queries;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Services;


public interface IServiceOperationQueryService
{
    Task<ServiceOperation?> Handle(GetServiceStatusByIdQuery query);
    Task<IEnumerable<ServiceOperation>> Handle(GetClientHistoryByTechnicianIdQuery query);
    
    Task<IEnumerable<ServiceOperation>> Handle(GetAllServiceOperationsQuery query);
}