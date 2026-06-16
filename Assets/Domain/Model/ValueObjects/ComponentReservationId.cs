using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record ComponentReservationId {
    public string Value { get; init; }

    private ComponentReservationId(string value) => Value = value;

    public static ComponentReservationId NewId() => new($"res-{Guid.NewGuid()}");

    public static ComponentReservationId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("res-"))
            throw new InvalidIdException("ComponentReservationId", value);
        return new ComponentReservationId(value);
    }

    public override string ToString() => Value;
}