namespace Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;

public record ReadingId
{
    public string Value { get; }
    private ReadingId(string value) => Value = value;
    public static ReadingId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ReadingId cannot be empty.");
        return new ReadingId(value);
    }
    public override string ToString() => Value;
}
