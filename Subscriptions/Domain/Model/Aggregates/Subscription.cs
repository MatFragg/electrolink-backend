using EntityFrameworkCore.CreatedUpdatedDate.Contracts;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
/// Represents a user's subscription. This is the root aggregate for managing subscription lifecycle.
/// </summary>
public class Subscription
{
    /// <summary>
    /// Unique identifier for the subscription.
    /// </summary>
    public SubscriptionId Id { get; private set; }
    
    /// <summary>
    /// The ID of the user associated with this subscription.
    /// </summary>
    public UserId UserId { get; private set; }

    /// <summary>
    /// The ID of the plan to which the user is subscribed.
    /// </summary>
    public PlanId PlanId { get; private set; }

    /// <summary>
    /// The current status of the subscription.
    /// </summary>
    public ESubscriptionStatus Status { get; private set; }

    /// <summary>
    /// The date when the subscription started.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// The date when the current subscription period is scheduled to end.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// The date when a cancelled subscription will actually cease to provide benefits.
    /// </summary>
    public DateTime? CancellationEffectiveDate { get; private set; }

    /// <summary>
    /// The date when the trial period (if any) ends.
    /// </summary>
    public DateTime? TrialEndsAt { get; private set; }

    /// <summary>
    /// Stripe's unique identifier for the customer.
    /// </summary>
    public string StripeCustomerId { get; private set; }

    /// <summary>
    /// Stripe's unique identifier for the subscription.
    /// </summary>
    public string StripeSubscriptionId { get; private set; }
    
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Current usage counter for limited benefits (e.g., number of service requests).
    /// </summary>
    public int CurrentUsage { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Subscription"/> class for a new subscription.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="planId">The ID of the plan.</param>
    /// <param name="startDate">The start date of the subscription.</param>
    /// <param name="endDate">The end date of the subscription.</param>
    /// <param name="stripeCustomerId">Stripe's customer ID.</param>
    /// <param name="stripeSubscriptionId">Stripe's subscription ID.</param>
    /// <param name="status">The initial status of the subscription.</param>
    /// <param name="trialEndsAt">Optional trial end date.</param>
    public Subscription(UserId userId, PlanId planId, DateTime startDate, DateTime endDate, string stripeCustomerId,
        string stripeSubscriptionId, ESubscriptionStatus status, DateTime? trialEndsAt = null)
    {
        Id = new SubscriptionId(Guid.NewGuid());
        UserId = userId;
        PlanId = planId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        StripeCustomerId = stripeCustomerId;
        StripeSubscriptionId = stripeSubscriptionId;
        TrialEndsAt = trialEndsAt;
        CurrentUsage = 0;
    }

    /// <summary>
    /// Private constructor for ORM or deserialization.
    /// </summary>
    private Subscription()
    {
    }   
    
    /// <summary>
    /// Adds a domain event to the subscription's event list.
    /// </summary>
    /// <param name="domainEvent"></param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    /// <summary>
    /// Clears all domain events from the subscription's event list.
    ///</summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Updates the status of the subscription.
    /// </summary>
    /// <param name="newStatus">The new status to set.</param>
    public void UpdateStatus(ESubscriptionStatus newStatus)
    { 
        var oldStatus = Status;
        if (oldStatus == newStatus) return;
            
        Status = newStatus;
        _domainEvents.Add(new SubscriptionStatusChangedEvent(
            Id.Value, 
            UserId, 
            oldStatus, 
            newStatus, 
            DateTime.UtcNow));
    }

    /// <summary>
    /// Schedules the subscription for cancellation, making it effective at the end of the current period.
    /// </summary>
    /// <param name="effectiveDate">The date when the cancellation becomes effective.</param>
    public void ScheduleCancellation(DateTime effectiveDate)
    {
        CancellationEffectiveDate = effectiveDate;
        // Optionally, change status to CancellationScheduled if needed
        // Status = ESubscriptionStatus.CancellationScheduled;
        
        if (Status != ESubscriptionStatus.Cancelled) 
            UpdateStatus(ESubscriptionStatus.Cancelled);
    }

    /// <summary>
    /// Activates a trial period for the subscription.
    /// </summary>
    /// <param name="trialEndDate">The date when the trial period ends.</param>
    public void ActivateTrial(DateTime trialEndDate)
    {
        var oldStatus = Status;
        Status = ESubscriptionStatus.Trial;
        TrialEndsAt = trialEndDate;
        CurrentUsage = 0;    // Reset usage for trial
        
        _domainEvents.Add(new SubscriptionStatusChangedEvent(
            Id.Value, 
            UserId, 
            oldStatus, 
            Status, 
            DateTime.UtcNow));
    }

    /// <summary>
    /// Increases the current usage count for a limited benefit.
    /// </summary>
    public void IncrementUsage()
    {
        CurrentUsage++;
    }

    /// <summary>
    /// Resets the current usage count for a limited benefit.
    /// </summary>
    public void ResetUsage()
    {
        CurrentUsage = 0;
    }

    /// <summary>
    /// Updates the Stripe subscription ID.
    /// </summary>
    /// <param name="newStripeSubscriptionId">The new Stripe subscription ID.</param>
    public void UpdateStripeSubscriptionId(string newStripeSubscriptionId)
    {
        StripeSubscriptionId = newStripeSubscriptionId;
    }

    /// <summary>
    /// Updates the subscription's plan.
    /// </summary>
    /// <param name="newPlanId">The ID of the new plan.</param>
    /// <param name="newEndDate">The new end date for the subscription.</param>
    public void ChangePlan(PlanId newPlanId, DateTime newEndDate)
    {
        var oldPlanId = PlanId;
        PlanId = newPlanId;
        EndDate = newEndDate;
        CurrentUsage = 0; // Reset usage on plan change
        TrialEndsAt = null; // Remove trial if changing plan
        // Raise a domain event
        _domainEvents.Add(new SubscriptionPlanChangedEvent(
            Id.Value, 
            UserId, 
            oldPlanId, 
            newPlanId, 
            DateTime.UtcNow));
    }

    /// <summary>
    /// Updates the subscription's end date.
    /// </summary>
    /// <param name="newEndDate">The new end date.</param>
    public void UpdateEndDate(DateTime newEndDate)
    {
        EndDate = newEndDate;
    }
    /// <summary>
    /// Updates the subscription's trial end date.
    /// </summary>
    /// <param name="newTrialEndsAt">The new trial end date, or null if trial is over/removed.</param>
    public void UpdateTrialEndsAt(DateTime? newTrialEndsAt)
    {
        TrialEndsAt = newTrialEndsAt;
        // Optionally, raise a domain event if this change is significant.
    }
    
}