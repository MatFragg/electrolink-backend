namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record InstallationRequestId
{
    public string Value { get; }

    private InstallationRequestId(string value) => Value = value;

    public static InstallationRequestId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("InstallationRequestId cannot be empty.");
        return new InstallationRequestId(value);
    }
}
