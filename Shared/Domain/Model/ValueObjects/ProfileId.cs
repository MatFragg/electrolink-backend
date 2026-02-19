using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record ProfileId
{
    public string Value { get; init; }

    private ProfileId(string value) => Value = value;

    public static ProfileId NewProfileId() => new($"prof-{Guid.NewGuid()}");

    public static ProfileId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("prof-"))
            throw new InvalidIdException("ProfileId", value);
        return new ProfileId(value);
    }

    public override string ToString() => Value;
}