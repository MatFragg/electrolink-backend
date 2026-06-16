using Hampcoders.Electrolink.API.Shared.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

public record DeviceId
{
    public string Value { get; init; }

    private DeviceId(string value) => Value = value;

    public static DeviceId NewDeviceId() => new($"dev-{Guid.NewGuid()}");

    public static DeviceId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("dev-"))
            throw new InvalidIdException("DeviceId", value);
        return new DeviceId(value);
    }

    public override string ToString() => Value;
}
