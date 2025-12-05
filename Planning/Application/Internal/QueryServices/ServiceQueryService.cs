using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;

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