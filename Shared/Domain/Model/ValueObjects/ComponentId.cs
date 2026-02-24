using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
public record ComponentId
{
    public string Value { get; init; }

    private ComponentId(string value) => Value = value;

    public static ComponentId NewComponentId() => new($"comp-{Guid.NewGuid()}");

    public static ComponentId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("comp-"))
            throw new InvalidIdException("ComponentId", value);
        return new ComponentId(value);
    }

    public override string ToString() => Value;
}