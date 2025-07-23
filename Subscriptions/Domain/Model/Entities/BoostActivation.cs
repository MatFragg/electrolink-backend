namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;

public class BoostActivation
{
    public DateTime? LastActivatedAt { get; private set; }

    public bool CanActivate(DateTime now)
    {
        return LastActivatedAt == null || (now - LastActivatedAt.Value).TotalHours >= 24;
    }

    public void Activate(DateTime now)
    {
        if (!CanActivate(now)) throw new InvalidOperationException("Boost already used recently.");
        LastActivatedAt = now;
    }
    
    public BoostActivation() { } 
}