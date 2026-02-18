namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record WarrantyPeriod
{
    public int Value { get; init; }
    public WarrantyUnit Unit { get; init; }
    
    public WarrantyPeriod() : this(0, WarrantyUnit.Months) { }
    
    public WarrantyPeriod(int value, WarrantyUnit unit)
    {
        Value = value;
        Unit = unit;
    }
    
    public int ToMonths() => Unit == WarrantyUnit.Years ? Value * 12 : Value;
}

public enum WarrantyUnit
{
    Months,
    Years
}

