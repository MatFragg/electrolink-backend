namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record AlertLogId
{
    public string Value { get; }
    private AlertLogId(string value) => Value = value;
    public static AlertLogId New() =>
        new($"alog-{Guid.NewGuid()}");
    public static AlertLogId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("AlertLogId cannot be empty.");
        return new AlertLogId(value);
    }
}
