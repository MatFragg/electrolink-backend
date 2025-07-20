namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

/// <summary>
/// Represents the unique identifier for an owner.
/// </summary>
public record OwnerId
{
    public Guid Id { get; }

    public OwnerId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Owner ID cannot be empty.", nameof(value));
        }
        Id = value;
    }

    public static OwnerId NewId() => new(Guid.NewGuid());
}