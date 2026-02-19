using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record PropertyId
{
    public string Value { get; init; }

    private PropertyId(string value) => Value = value;

    public static PropertyId NewPropertyId() => new($"prop-{Guid.NewGuid()}");

    public static PropertyId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("prop-"))
            throw new InvalidIdException("PropertyId", value);
        return new PropertyId(value);
    }

    public override string ToString() => Value;
}