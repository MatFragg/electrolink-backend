using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record HomeownerId
{
    public string Value { get; init; }

    private HomeownerId(string value) => Value = value;

    public static HomeownerId NewHomeownerId() => new($"ho-{Guid.NewGuid()}");

    public static HomeownerId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("ho-"))
            throw new InvalidIdException("HomeownerId", value);
        return new HomeownerId(value);
    }

    public override string ToString() => Value;
}