using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal;

public class PlanCommandService(IPlanRepository planRepository, IUnitOfWork unitOfWork) : IPlanCommandService
{
    public async Task<Guid> Handle(CreatePlanCommand command)
    {
        var plan = new Plan(
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            command.MonetizationType,
            command.IsDefault,
            command.UsageLimit
        );

        await planRepository.AddAsync(plan);
        await unitOfWork.CompleteAsync();
        return plan.Id.Value;
    }

    public async Task Handle(UpdatePlanCommand command)
    {
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan is null) return;

        plan.UpdateDetails(
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            Enum.Parse<MonetizationType>(command.MonetizationType),
            command.IsDefault,
            command.UsageLimit
        );

        planRepository.Update(plan);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(DeletePlanCommand command)
    {
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan != null)
        {
            planRepository.Remove(plan);
            await unitOfWork.CompleteAsync();
        }
    }

    public async Task Handle(MarkPlanAsDefaultCommand command)
    {
        var currentDefault = await planRepository.FindDefaultAsync();
        if (currentDefault != null)
        {
            currentDefault.UnmarkAsDefault();
            planRepository.Update(currentDefault);
        }

        var target = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (target != null)
        {
            target.MarkAsDefault();
            planRepository.Update(target);
        }

        await unitOfWork.CompleteAsync();
    }
}
