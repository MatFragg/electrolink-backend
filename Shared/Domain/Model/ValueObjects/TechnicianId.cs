using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record TechnicianId
{
    public string Value { get; init; }

    private TechnicianId(string value) => Value = value;

    public static TechnicianId NewTechnicianId() => new($"tech-{Guid.NewGuid()}");

    public static TechnicianId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("tech-"))
            throw new InvalidIdException("TechnicianId", value);
        return new TechnicianId(value);
    }

    public override string ToString() => Value;
}