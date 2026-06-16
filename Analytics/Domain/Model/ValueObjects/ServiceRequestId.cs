namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record ServiceRequestId
{
    public string Value { get; }
    private ServiceRequestId(string value) => Value = value;
    public static ServiceRequestId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ServiceRequestId cannot be empty.");
        return new ServiceRequestId(value);
    }
}
