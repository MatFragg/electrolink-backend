namespace Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;

public record StreamId
{
    public string Value { get; }
    private StreamId(string value) => Value = value;
    public static StreamId NewId()  => new($"stream-{Guid.NewGuid()}");
    public static StreamId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("StreamId cannot be empty.");
        return new StreamId(value);
    }
    public override string ToString() => Value;
}
