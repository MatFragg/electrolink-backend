using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record TechnicianInventoryId
{
    public string Value { get; init; }

    private TechnicianInventoryId(string value) => Value = value;

    public static TechnicianInventoryId NewTechnicianInventoryId() => new($"inv-{Guid.NewGuid()}");

    public static TechnicianInventoryId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("inv-"))
            throw new InvalidIdException("TechnicianInventoryId", value);
        return new TechnicianInventoryId(value);
    }

    public override string ToString() => Value;
}