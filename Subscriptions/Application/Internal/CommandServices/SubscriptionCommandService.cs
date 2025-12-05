using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPlanRepository planRepository,
    IUnitOfWork unitOfWork,
    ExternalProfileService externalProfileService,
    ExternalIamService externalIamService,
    IMediator mediator
) : ISubscriptionCommandService
{
    public async Task<Guid> Handle(CreateSubscriptionCommand command)
    {
        // 1. Validate User existence
        if (!await externalIamService.UserExistsAsync(command.UserId))
            throw new ArgumentException($"User with ID {command.UserId} does not exist in IAM.");
        
        // 2. Validate plan existence
        var plan = await planRepository.FindByIdAsync(command.PlanId);
        if (plan == null)
            throw new ArgumentException($"Plan with ID {command.PlanId} not found.");
        
        // 3. Validate Technician existence and retrieve their information
        if (plan.TargetRole == EUserRole.Technician)
        {
            var technicianInfo = await externalProfileService.FetchTechnicianInfoByProfileIdAsync(command.UserId);
            if (technicianInfo is null)
                throw new InvalidOperationException($"Technician not found for profile {command.UserId}");
        }

        // 4. Create new subscription aggregate
        var subscription = new Subscription(
            new UserId(command.UserId),
            new PlanId(command.PlanId),
            command.StartDate,
            command.EndDate,
            command.StripeCustomerId,
            command.StripeSubscriptionId,
            command.InitialStatus,
            command.TrialEndsAt
        );

        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        subscription.ClearDomainEvents();
        
        return subscription.Id.Value;
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(UpdateSubscriptionStatusCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null) return null;

        subscription.UpdateStatus(command.NewStatus);
        subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();

        // Publish domain event (e.g., SubscriptionStatusChangedEvent)
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
        
        return subscription.Id.Value; 
    }

    /// <inheritdoc/>
    public async Task Handle(CancelSubscriptionCommand command) // Void return
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null) throw new ArgumentException($"Subscription with ID {command.SubscriptionId} not found.");

        subscription.ScheduleCancellation(command.CancellationEffectiveDate);
        await unitOfWork.CompleteAsync();

        // Publish domain event
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(ActivateTrialCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null) return null;

        subscription.ActivateTrial(command.TrialEndDate);
        // subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();

        // Publish domain event 
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();

        return subscription.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(IncrementSubscriptionUsageCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null) return null;

        subscription.IncrementUsage();
        //subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();
        
        
        // 📌 TODO: Publish (e.g., SubscriptionUsageIncrementedEvent) domain event if it is needed.
        // await mediator.Publish(new SubscriptionUsageIncrementedEvent(subscription.Id.Value, subscription.UsageCount, DateTime.UtcNow));
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
        return subscription.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(ResetSubscriptionUsageCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null) return null;

        subscription.ResetUsage();
        // subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }
        subscription.ClearDomainEvents();
        return subscription.Id.Value; // Return the ID
    }

    /// <inheritdoc/>
    public async Task Handle(ApplyDiscountCommand command) // Void return
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null) throw new ArgumentException($"Subscription with ID {command.SubscriptionId} not found.");

        // TODO: Implement discount logic (e.g., interact with Stripe API to apply coupon)
        // This would likely involve updating a discount related field on the subscription or a Stripe call.
        // _subscriptionRepository.Update(subscription); // Changed to void if subscription state changes
        await unitOfWork.CompleteAsync();

        // 📌 TODO: Publish domain event (e.g., DiscountAppliedEvent) if it is needed to apply a discount.
        // await _mediator.Publish(new DiscountAppliedEvent(subscription.Id.Value, command.DiscountCode, DateTime.UtcNow));
        
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
    }

    /// <inheritdoc/>
    public async Task<Guid?> Handle(ChangeSubscriptionPlanCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null) return null;

        var oldPlanId = subscription.PlanId;
        var newPlanId = new PlanId(command.NewPlanId);
        var newPlan = await planRepository.FindByIdAsync(newPlanId);
        if (newPlan == null) 
            throw new ArgumentException($"New plan with ID {newPlanId} not found.");
        
        subscription.ChangePlan(newPlanId, command.NewEndDate);
        subscription.UpdateStripeSubscriptionId(command.StripeSubscriptionId); // Update if Stripe sub ID changes with plan
        //subscriptionRepository.Update(subscription); // Changed to void
        await unitOfWork.CompleteAsync();

        // Publish domain event (e.g., SubscriptionPlanChangedEvent)
        foreach (var domainEvent in subscription.DomainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        subscription.ClearDomainEvents();
        
        return subscription.Id.Value; // Return the ID
    }
    
    
}