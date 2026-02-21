namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;


public record TechnicianId(string Value)
{
    public static implicit operator string(TechnicianId id) => id.Value;
    public static implicit operator TechnicianId(int value) => new(value);
    public override string ToString() => Value.ToString();
}