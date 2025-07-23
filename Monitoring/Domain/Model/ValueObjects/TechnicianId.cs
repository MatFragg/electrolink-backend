namespace Hamcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;


public record TechnicianId(int Value)
{
    public static implicit operator int(TechnicianId id) => id.Value;
    public static implicit operator TechnicianId(int value) => new(value);
    public override string ToString() => Value.ToString();
}