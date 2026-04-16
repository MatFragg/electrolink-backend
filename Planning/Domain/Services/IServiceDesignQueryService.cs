using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IServiceDesignQueryService
{
    Task<ServiceCatalog?> Handle(GetServiceCatalogQuery query);
    Task<ServiceRecipe?> Handle(GetServiceRecipeDetailsQuery query);
    Task<RequestEligibility> Handle(GetRequestEligibilityQuery query);  
    Task<IEnumerable<AvailableService>> Handle(GetAvailableServicesQuery query); 
    Task<ServiceRequest?> Handle(GetServiceRequestSummaryQuery query);
    Task<MatchingQueue> Handle(GetMatchingQueueQuery query);
    Task<ServiceRequest?> Handle(GetServiceRequestByIdQuery query);
    
}