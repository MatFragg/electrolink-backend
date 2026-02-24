using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record ServiceId
{
    public string Value { get; init; }

    private ServiceId(string value) => Value = value;

    public static ServiceId NewServiceId() => new($"serv-{Guid.NewGuid()}");

    public static ServiceId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("serv-"))
            throw new InvalidIdException("ServiceId", value);
        return new ServiceId(value);
    }

    public override string ToString() => Value;
}