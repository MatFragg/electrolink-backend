namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record SourceEventId
{
    public string Value { get; }
    private SourceEventId(string value) => Value = value;
    public static SourceEventId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SourceEventId cannot be empty.");
        return new SourceEventId(value);
    }
}
