namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;

public class PremiumAccess
{
    public DateTime ValidUntil { get; private set; }

    public PremiumAccess(DateTime validUntil)
    {
        ValidUntil = validUntil;
    }

    public bool IsActive(DateTime now) => now <= ValidUntil;
}