namespace Hampcoders.Electrolink.API.Processing.Domain.Model.ValueObjects;

public record AnomalyRecordId
{
    public string Value { get; }
    private AnomalyRecordId(string value) => Value = value;
    public static AnomalyRecordId NewId()  => new($"anm-{Guid.NewGuid()}");
    public static AnomalyRecordId From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("AnomalyRecordId cannot be empty.");
        return new AnomalyRecordId(value);
    }
    public override string ToString() => Value;
}
