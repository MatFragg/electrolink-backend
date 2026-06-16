using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record RelayActionId
{
    public string Value { get; init; }

    private RelayActionId(string value) => Value = value;

    public static RelayActionId NewRelayActionId() => new($"rel-{Guid.NewGuid()}");

    public static RelayActionId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("rel-"))
            throw new InvalidIdException("RelayActionId", value);
        return new RelayActionId(value);
    }

    public override string ToString() => Value;
}
