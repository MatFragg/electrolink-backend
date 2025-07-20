using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Services;

public interface IServiceQueryService
{
    Task<Service?> Handle(GetServiceByIdQuery query);
    Task<IEnumerable<Service>> Handle(GetAllVisibleServicesQuery query);
}