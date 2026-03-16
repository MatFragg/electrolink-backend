using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record ComponentTypeId{
    public string Value { get; init; }

    private ComponentTypeId(string value) => Value = value;

    public static ComponentTypeId NewComponentTypeId() => new($"ctype-{Guid.NewGuid()}");

    public static ComponentTypeId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("ctype-"))
            throw new InvalidIdException("ComponentTypeId", value);
        return new ComponentTypeId(value);
    }

    public override string ToString() => Value;
}