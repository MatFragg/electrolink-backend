using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record ReservationItemId {
    public string Value { get; init; }

    private ReservationItemId(string value) => Value = value;

    public static ReservationItemId NewReservationItemId() => new($"ritem-{Guid.NewGuid()}");

    public static ReservationItemId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("ritem-"))
            throw new InvalidIdException("ReservationItemId", value);
        return new ReservationItemId(value);
    }

    public override string ToString() => Value;
}