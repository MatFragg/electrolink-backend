using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.API.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.API.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.API.Application.Internal.QueryServices;

public class ServiceQueryService(IServiceRepository serviceRepository) : IServiceQueryService
{
    public async Task<Service?> Handle(GetServiceByIdQuery query)
    {
        return await serviceRepository.FindByIdAsync(query.ServiceId);
    }

    public async Task<IEnumerable<Service>> Handle(GetAllVisibleServicesQuery query)
    {
        return await serviceRepository.ListAllVisibleAsync();
    }
}