namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Entities;

public class CertificationRequest
{
    public bool IsVerified { get; private set; }
    public DateTime? VerifiedAt { get; private set; }

    public void Activate()
    {
        IsVerified = true;
        VerifiedAt = DateTime.UtcNow;
    }
}