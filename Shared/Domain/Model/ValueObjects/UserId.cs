using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record UserId
{
    public string Value { get; init; }

    private UserId(string value) => Value = value;

    public static UserId NewUserId() => new($"us-{Guid.NewGuid()}");

    public static UserId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("us-"))
            throw new InvalidIdException("UserId", value);
        return new UserId(value);
    }

    public override string ToString() => Value;
}