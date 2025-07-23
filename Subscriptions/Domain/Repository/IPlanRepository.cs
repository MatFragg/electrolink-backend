using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;

public interface IPlanRepository : IBaseRepository<Plan>
{
    Task<Plan?> FindByIdAsync(PlanId id);
    Task<Plan?> FindDefaultAsync();
}