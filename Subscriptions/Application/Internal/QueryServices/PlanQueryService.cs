using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class PlanQueryService(IPlanRepository _planRepository) : IPlanQueryService
{
    public async Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query)
    {
        return await _planRepository.ListAsync();
    }

    public async Task<Plan?> Handle(GetPlanByIdQuery query)
    {
        return await _planRepository.FindByIdAsync(new PlanId(query.PlanId));
    }

    public async Task<Plan?> Handle(GetDefaultPlanQuery query)
    {
        return await _planRepository.FindDefaultAsync();
    }
}