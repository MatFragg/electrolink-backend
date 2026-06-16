namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record AlertEntryId
{
    public string Value { get; }
    private AlertEntryId(string value) => Value = value;
    public static AlertEntryId New() =>
        new($"alrt-{Guid.NewGuid()}");
    public static AlertEntryId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("AlertEntryId cannot be empty.");
        return new AlertEntryId(value);
    }
}
