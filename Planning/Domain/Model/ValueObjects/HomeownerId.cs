namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record HomeownerId(Guid Id)
{
    public HomeownerId() : this(Guid.Empty) { }
    public static HomeownerId NewId() => new(Guid.NewGuid());
}

