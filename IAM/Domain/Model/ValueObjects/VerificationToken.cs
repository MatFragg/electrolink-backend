namespace Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;

public record VerificationToken
{
    public string Value { get; }
    public DateTime ExpiresAt { get; }

    private VerificationToken(string value, DateTime expiresAt)
    {
        Value = value;
        ExpiresAt = expiresAt;
    }

    public static VerificationToken Generate() =>
        new($"tok-{Guid.NewGuid()}", DateTime.UtcNow.AddHours(24));

    public bool IsValid(string token) =>
        Value == token && DateTime.UtcNow < ExpiresAt;
}