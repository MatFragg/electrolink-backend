using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hamcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hamcoders.Electrolink.API.Subscriptions.Application.Internal;

public class PlanCommandService(IPlanRepository _planRepository, IUnitOfWork unitOfWork) : IPlanCommandService
{
    public async Task<Guid> Handle(CreatePlanCommand command)
    {
        var plan = new Plan(
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            command.MonetizationType,
            command.IsDefault
        );

        await _planRepository.AddAsync(plan);
        await unitOfWork.CompleteAsync();
        return plan.Id.Value;
    }

    public async Task Handle(UpdatePlanCommand command)
    {
        var plan = await _planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan is null) return;

        plan.UpdateDetails(
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            Enum.Parse<MonetizationType>(command.MonetizationType),
            command.IsDefault
        );

        _planRepository.Update(plan);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(DeletePlanCommand command)
    {
        var plan = await _planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan != null)
        {
            _planRepository.Remove(plan);
            await unitOfWork.CompleteAsync();
        }
    }

    public async Task Handle(MarkPlanAsDefaultCommand command)
    {
        var currentDefault = await _planRepository.FindDefaultAsync();
        if (currentDefault != null)
        {
            currentDefault.UnmarkAsDefault();
            _planRepository.Update(currentDefault);
        }

        var target = await _planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (target != null)
        {
            target.MarkAsDefault();
            _planRepository.Update(target);
        }

        await unitOfWork.CompleteAsync();
    }
}
