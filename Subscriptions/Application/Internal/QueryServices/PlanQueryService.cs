using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;

public class PlanQueryService(IPlanRepository planRepository) : IPlanQueryService
{
    public async Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query)
    {
        return await planRepository.ListAsync();
    }

    /// <inheritdoc/>
    public async Task<Plan?> Handle(GetDefaultPlanByRoleQuery query)
    {
        return await planRepository.FindDefaultPlanByRoleAsync(query.Role);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Plan>> Handle(GetPlansByRoleQuery query)
    {
        return await planRepository.ListPlansByRoleAsync(query.Role);
    }

    /// <inheritdoc/>
    public async Task<Plan?> Handle(GetPlanByIdQuery query)
    {
        return await planRepository.FindByIdAsync(new PlanId(query.PlanId));
    }

    /// <inheritdoc/>
    public async Task<Plan?> Handle(GetDefaultPlanQuery query)
    {
        return await planRepository.FindDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<Plan?> Handle(GetPlanDetailsQuery query)
    {
        return await planRepository.FindByIdAsync(new PlanId(query.PlanId));
    }
}