using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Services;

public interface IServiceQueryService
{
    Task<Service?> Handle(GetServiceByIdQuery query);
    Task<IEnumerable<Service>> Handle(GetAllVisibleServicesQuery query);
}