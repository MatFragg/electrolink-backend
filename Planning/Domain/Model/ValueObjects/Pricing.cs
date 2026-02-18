namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record Pricing
{
    public Money MaterialsEstimate { get; init; }
    public Money LaborCost { get; init; }
    public Money TotalPrice { get; init; }
    
    public Pricing() : this(Money.Zero(), Money.Zero(), Money.Zero()) { }
    
    public Pricing(Money materialsEstimate, Money laborCost, Money totalPrice)
    {
        if (totalPrice.Amount < materialsEstimate.Amount + laborCost.Amount)
            throw new ArgumentException("Total price must be >= materials + labor");
        
        MaterialsEstimate = materialsEstimate;
        LaborCost = laborCost;
        TotalPrice = totalPrice;
    }
}

