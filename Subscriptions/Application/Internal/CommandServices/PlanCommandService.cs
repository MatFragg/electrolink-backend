using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class PlanCommandService(IPlanRepository planRepository, IUnitOfWork unitOfWork, IMediator mediator) : IPlanCommandService
{
    public async Task<Guid> Handle(CreatePlanCommand command)
    {
        var plan = new Plan(
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            command.MonetizationType,
            command.TargetRole,
            command.IsDefault,
            command.Benefits,
            command.GatewayPriceId
        );

        await planRepository.AddAsync(plan);
        await unitOfWork.CompleteAsync();

        // Publish domain event
        foreach (var domainEvent in plan.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        plan.ClearDomainEvents();
        
        return plan.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(UpdatePlanCommand command)
    {
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan == null) return null;

        plan.UpdateDetails(
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            command.MonetizationType,
            command.TargetRole,
            command.IsDefault,
            command.Benefits
        );

        planRepository.Update(plan); // Changed to void
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in plan.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        plan.ClearDomainEvents();   
        
        return plan.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task Handle(DeletePlanCommand command) // Void return
    {
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan == null) throw new ArgumentException($"Plan with ID {command.PlanId} not found.");

        planRepository.Remove(plan); // Changed to void
        await unitOfWork.CompleteAsync();

        // Publish domain event (e.g., PlanDeletedEvent)  if needed, notify meaningful changes.
        // await mediator.Publish(new PlanDeletedEvent(command.PlanId, DateTime.UtcNow));

    }
}
