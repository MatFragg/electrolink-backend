using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;

namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface IPlanQueryService
{
    Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query);
    Task<Plan?> Handle(GetPlanByIdQuery query);
    Task<Plan?> Handle(GetDefaultPlanQuery query);
}