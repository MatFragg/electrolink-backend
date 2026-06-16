namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record IoTDeviceId
{
    public string Value { get; }

    private IoTDeviceId(string value) => Value = value;

    public static IoTDeviceId NewId() => new($"dev-{Guid.NewGuid()}");

    public static IoTDeviceId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("IoTDeviceId cannot be empty.");
        return new IoTDeviceId(value);
    }
}
