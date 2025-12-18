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
    IPaymentGatewayService paymentGateway,
    IUnitOfWork unitOfWork,
    ExternalProfileService externalProfileService,
    ExternalIamService externalIamService,
    IMediator mediator
) : ISubscriptionCommandService
{
    public async Task<Guid> Handle(CreateSubscriptionCommand command)
    {
        // 1. Validate User existence
        if (!await externalIamService.UserExistsAsync(command.UserId.Value))
            throw new ArgumentException($"User with ID {command.UserId} does not exist in IAM.");
        
        // 2. Validate plan existence
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId));
        if (plan == null)
            throw new ArgumentException($"Plan with ID {command.PlanId} not found.");
        
        // 3. Validate Profile existence
        if (!await externalProfileService.ProfileExistsAsync(command.UserId.Value))
            throw new ArgumentException($"Profile not found for User {command.UserId.Value}.");
        
        // 4. Validate Technician existence and retrieve their information
        if (plan.TargetRole == EUserRole.Technician)
        {
            var technicianInfo = await externalProfileService.FetchTechnicianInfoByProfileIdAsync(command.UserId.Value);
            if (technicianInfo is null)
                throw new InvalidOperationException($"Technician not found for profile {command.UserId.Value}.");
        }

        // 4. Create new subscription aggregate
        var subscription = new Subscription(
            new UserId(command.UserId.Value),
            new PlanId(command.PlanId),
            command.StartDate,
            command.EndDate,
            command.GatewayCustomerId,
            command.GatewaySubscriptionId,
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
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
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
    public async Task<Guid?> Handle(ActivateTrialCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
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
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
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
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
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
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId));
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
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId.Value));
        if (subscription == null) return null;

        var oldPlanId = subscription.PlanId;
        var newPlanId = new PlanId(command.NewPlanId);
        var newPlan = await planRepository.FindByIdAsync(newPlanId);
        if (newPlan == null) 
            throw new ArgumentException($"New plan with ID {newPlanId} not found.");
        
        subscription.ChangePlan(newPlanId, command.NewEndDate);
        subscription.UpdateStripeSubscriptionId(command.GatewaySubscriptionId); // Update if Stripe sub ID changes with plan
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
    
    
    /// <summary>
    /// Creates a checkout session to initiate subscription.
    /// </summary>
    public async Task<string> Handle(CreateCheckoutSessionCommand command)
    {
        // 1. Validate that the user exists
        if (!await externalIamService.UserExistsAsync(command.UserId.Value))
            throw new ArgumentException($"User {command.UserId} not found");

        // 2. Fetch profile info
        var email = await externalProfileService.FetchProfileEmail(command.UserId.Value);
        var fullName = await externalProfileService.FetchProfileFullName(command.UserId.Value);
        
        if (string.IsNullOrEmpty(email))
            throw new InvalidOperationException($"Profile for User {command.UserId.Value} requires an email to subscribe.");
        
        // 2. Validate that the plan exists
        var plan = await planRepository.FindByIdAsync(new PlanId(command.PlanId.Value));
        if (plan == null)
            throw new ArgumentException($"Plan {command.PlanId.Value} not found");

        if (plan.GatewayPriceId == null || string.IsNullOrEmpty(plan.GatewayPriceId.Value))
            throw new InvalidOperationException($"Plan {plan.Name} does not have a Stripe Price ID configured");

        // 3. Verifies if the user already has an active subscription
        var existingSubscription = await subscriptionRepository.FindActiveByUserIdAsync(new UserId(command.UserId.Value));

        if (existingSubscription != null)
            throw new InvalidOperationException(
                $"User {command.UserId.Value} already has an active subscription");

        // 4. Creates a Customer in Stripe
        var stripeCustomerId = await paymentGateway.CreateOrGetCustomerAsync(
            command.UserId.Value,
            email,
            fullName);

        // 5. Creates a Checkout session
        var checkoutUrl = await paymentGateway.CreateCheckoutSessionAsync(
            stripeCustomerId,
            new PaymentGatewayPriceId(plan.GatewayPriceId.Value),
            command.SuccessUrl,
            command.CancelUrl,
            command.TrialPeriodDays);


        return checkoutUrl;
    }

    /// <summary>
    /// Processes a payment transaction and updates subscription status.
    /// This is the CORRECT place for payment processing since it affects Subscription.
    /// </summary>
    public async Task<Guid> Handle(ProcessPaymentCommand command)
    {

        var subscriptionId = new SubscriptionId(command.SubscriptionId.Value);
        var subscription = await subscriptionRepository.FindByIdAsync(subscriptionId);

        if (subscription == null)
            throw new ArgumentException($"Subscription {subscriptionId} not found");

        // Create payment transaction record
        var transaction = new PaymentTransaction(
            subscriptionId,
            command.Amount ?? 0m,
            command.Currency,
            command.TransactionDate,
            command.Status,
            command.GatewayTransactionId,
            command.Message);

        // Update subscription based on payment result
        if (command.Status == EPaymentStatus.Success)
        {
            subscription.UpdateStatus(ESubscriptionStatus.Active);
            subscription.UpdateEndDate(subscription.EndDate.AddMonths(1));
        }
        else if (command.Status == EPaymentStatus.Failed)
        {
            subscription.UpdateStatus(ESubscriptionStatus.PaymentDue);
        }

        // Persist
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

        // Publish domain events
        foreach (var domainEvent in subscription.DomainEvents)
            await mediator.Publish(domainEvent);

        subscription.ClearDomainEvents();
        return transaction.Id;
    }

    /// <summary>
    /// Cancels a subscription in the payment gateway.
    /// </summary>
    public async Task Handle(CancelSubscriptionCommand command)
    {
        // 1. Find the subscription
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId.Value));
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId} not found");

        var stripeSubscriptionId = new PaymentGatewaySubscriptionId(subscription.GatewaySubscriptionId.Value);

        // 2. Cancels the subscription in Stripe
        if (command.Immediately)
        {
            await paymentGateway.CancelSubscriptionImmediatelyAsync(stripeSubscriptionId);
            subscription.UpdateStatus(ESubscriptionStatus.Cancelled);
        }
        else
        {
            var cancelAt = await paymentGateway.CancelSubscriptionAtPeriodEndAsync(stripeSubscriptionId);
            subscription.ScheduleCancellation(cancelAt);
        }

        // 3. Persist changes
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
    }

    /// <summary>
    /// Changes the plan of a subscription.
    /// </summary>
    public async Task Handle(ChangeSubscriptionPlanInGatewayCommand command)
    {
        // 1. Find the subscription
        var subscription = await subscriptionRepository.FindByIdAsync(new SubscriptionId(command.SubscriptionId.Value));
        if (subscription == null)
            throw new ArgumentException($"Subscription {command.SubscriptionId} not found");

        // 2. Find the new plan
        var newPlanId = new PlanId(command.NewPlanId.Value);
        var newPlan = await planRepository.FindByIdAsync(newPlanId);
        if (newPlan == null)
            throw new ArgumentException($"Plan {command.NewPlanId} not found");

        if (newPlan.GatewayPriceId == null || string.IsNullOrEmpty(newPlan.GatewayPriceId.Value))
            throw new InvalidOperationException($"Plan {newPlan.Name} does not have a Stripe Price ID");

        // 3. Updates in Stripe
        var stripeSubscriptionId = new PaymentGatewaySubscriptionId(subscription.GatewaySubscriptionId.Value);
        var updatedStripeSubscription = await paymentGateway.UpdateSubscriptionPlanAsync(
            stripeSubscriptionId,
            new PaymentGatewayPriceId(newPlan.GatewayPriceId.Value),
            command.ProrationBehavior);

        // 4. Update in our DB
        subscription.ChangePlan(
            newPlanId,
            updatedStripeSubscription.currentPeriodEnd);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
    }

    /// <summary>
    /// Syncs subscription from payment gateway.
    /// </summary>
    public async Task Handle(SyncSubscriptionFromGatewayCommand command)
    {
        var subscription = await subscriptionRepository
            .FindByPaymentGatewaySubscriptionIdAsync(command.GatewaySubscriptionId);

        if (subscription == null)
        {
            throw new ArgumentNullException(
                nameof(command.GatewaySubscriptionId),
                $"Subscription not found for gateway ID {command.GatewaySubscriptionId}");
        }

        // Map gateway status to domain status
        var newStatus = MapGatewayStatusToSubscriptionStatus(command.Status);

        // Update subscription
        subscription.UpdateStatus(newStatus);
        subscription.UpdateEndDate(command.CurrentPeriodEnd);
        subscription.UpdateTrialEndsAt(command.TrialEnd);

        if (command.CancelAtPeriodEnd && command.CancelAt.HasValue)
        {
            subscription.ScheduleCancellation(command.CancelAt.Value);
        }

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();

    }
    
    
    public async Task<string> Handle(CreateBillingPortalSessionCommand command)
    {
        // 1. Buscar la suscripción activa del usuario
        var subscription = await subscriptionRepository.FindActiveByUserIdAsync(command.UserId);
        if (subscription == null)
            throw new ArgumentException($"No se encontró una suscripción activa para el usuario {command.UserId.Value}.");

        // 2. Obtener el customer ID de Stripe
        var stripeCustomerId = subscription.GatewayCustomerId;

        // 3. Crear la sesión de billing portal usando el servicio de pago
        var portalUrl = await paymentGateway.CreateBillingPortalSessionAsync(
            new PaymentGatewayCustomerId(stripeCustomerId.Value),
            command.ReturnUrl);

        return portalUrl;
    }
    
    /// <summary>
    /// Maps payment gateway status to domain subscription status.
    /// This is OK here because it's mapping from a generic "gateway status" string.
    /// </summary>
    private ESubscriptionStatus MapGatewayStatusToSubscriptionStatus(string gatewayStatus)
    {
        return gatewayStatus.ToLowerInvariant() switch
        {
            "active" => ESubscriptionStatus.Active,
            "trialing" => ESubscriptionStatus.Trial,
            "past_due" => ESubscriptionStatus.PaymentDue,
            "canceled" => ESubscriptionStatus.Cancelled,
            "unpaid" => ESubscriptionStatus.Expired,
            "incomplete" or "incomplete_expired" => ESubscriptionStatus.Pending,
            _ => ESubscriptionStatus.Pending
        };
    }

    
}