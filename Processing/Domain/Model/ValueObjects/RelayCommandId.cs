namespace Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;

public record RelayCommandId
{
    public string Value { get; }
    private RelayCommandId(string value) => Value = value;
    public static RelayCommandId NewId()  => new($"relay-{Guid.NewGuid()}");
    public static RelayCommandId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("RelayCommandId cannot be empty.");
        return new RelayCommandId(value);
    }
    public override string ToString() => Value;
}
