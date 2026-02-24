using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record ComponentStockId{
    public string Value { get; init; }

    private ComponentStockId(string value) => Value = value;

    public static ComponentStockId NewComponentStockId() => new($"compstock-{Guid.NewGuid()}");

    public static ComponentStockId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("compstock-"))
            throw new InvalidIdException("ComponentStockId", value);
        return new ComponentStockId(value);
    }

    public override string ToString() => Value;
}