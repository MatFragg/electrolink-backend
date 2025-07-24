using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface IPlanQueryService
{
    Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query);
    Task<Plan?> Handle(GetPlanByIdQuery query);
    Task<Plan?> Handle(GetDefaultPlanQuery query);
}