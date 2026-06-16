namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record SerialNumber
{
    public string Value { get; }

    private SerialNumber(string value) => Value = value;

    public static SerialNumber From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SerialNumber cannot be empty.");
        if (value.Length > 100)
            throw new ArgumentException("SerialNumber cannot exceed 100 characters.");
        return new SerialNumber(value);
    }
}
