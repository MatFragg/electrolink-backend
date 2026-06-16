namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record TechnicianMetricsId
{
    public string Value { get; }
    private TechnicianMetricsId(string value) => Value = value;
    public static TechnicianMetricsId New() =>
        new($"tmet-{Guid.NewGuid()}");
    public static TechnicianMetricsId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("TechnicianMetricsId cannot be empty.");
        return new TechnicianMetricsId(value);
    }
}
