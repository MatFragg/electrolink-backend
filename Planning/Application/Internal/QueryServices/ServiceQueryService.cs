using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;

public class ServiceQueryService(IServiceRepository serviceRepository) : IServiceQueryService
{
    public async Task<Service?> Handle(GetServiceByIdQuery query)
    {
        var serviceId = new ServiceId(query.ServiceId);
        return await serviceRepository.FindByIdAsync(serviceId);
    }

    public async Task<IEnumerable<Service>> Handle(GetAllVisibleServicesQuery query)
    {
        return await serviceRepository.ListAllVisibleAsync();
    }
}