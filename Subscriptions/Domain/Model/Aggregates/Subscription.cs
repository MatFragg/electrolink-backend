using EntityFrameworkCore.CreatedUpdatedDate.Contracts;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

public class Subscription : IEntityWithCreatedUpdatedDate
{
    public SubscriptionId Id { get; private set; }
    public UserId UserId { get; private set; }
    public PlanId PlanId { get; private set; }
    public ESubscriptionsStatus Status { get; private set; }
    public DateTime ActivatedAt { get; private set; } = new();

    public PremiumAccess? PremiumAccess { get; private set; }
    public CertificationRequest? Certification { get; private set; }
    public BoostActivation Boost { get; private set; } 

    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public int CurrentUsage { get; private set; } 

    private Subscription() {}

    public Subscription(UserId userId, PlanId planId)
    {
        Id = new SubscriptionId(Guid.NewGuid());
        UserId = userId;
        PlanId = planId;
        Status = ESubscriptionsStatus.Active;
        ActivatedAt = DateTime.UtcNow;
        Certification = new CertificationRequest();
        Boost = new BoostActivation();
        CurrentUsage = 0;
    }

    public void Cancel() => Status = ESubscriptionsStatus.Cancelled;

    public void Pause() => Status = ESubscriptionsStatus.Paused;

    public void Resume() => Status = ESubscriptionsStatus.Active;

    public void GrantPremiumAccess(DateTime until)
    {
        PremiumAccess = new PremiumAccess(until);
    }

    public void VerifyCertification(DateTime now)
    {
        if (Status == ESubscriptionsStatus.Active && (now - ActivatedAt).TotalDays >= 30)
            Certification!.Activate();
    }
    
    public void IncrementUsage(int amount = 1) 
    {
        CurrentUsage += amount;
    }

    public bool CanActivateBoost(DateTime now) => Boost!.CanActivate(now);

    public void ActivateBoost(DateTime now) => Boost!.Activate(now);
}