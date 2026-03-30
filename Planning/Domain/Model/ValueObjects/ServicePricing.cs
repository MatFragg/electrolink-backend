using System.Text.Json.Serialization;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ServicePricing
{
    public Money MaterialsEstimate { get; }
    public Money LaborCost { get; }
    public Money TotalPrice { get; }

    private ServicePricing() { }
    
    [JsonConstructor]
    public ServicePricing(Money materialsEstimate, Money laborCost, Money totalPrice)
    {
        MaterialsEstimate = materialsEstimate;
        LaborCost = laborCost;
        TotalPrice = totalPrice;
    }

    public static ServicePricing Create(Money materialsEstimate, Money laborCost, Money totalPrice)
    {
        var minTotal = materialsEstimate.Add(laborCost);

        if (totalPrice.Amount < minTotal.Amount)
            throw new InvalidOperationException("El precio total debe ser >= materiales + labor.");

        if (totalPrice.Amount < 0.01m)
            throw new InvalidOperationException("El precio total debe ser mayor que 0.");

        return new ServicePricing(materialsEstimate, laborCost, totalPrice);
    }

    /// <summary>
    /// Valida que el nuevo precio no exceda el 20% de aumento respecto al precio actual.
    /// Hotspot 7: variaciones de precio máximo 20% cuando hay servicios activos
    /// </summary>
    public bool IsPriceIncreaseOver20Percent(Money newTotal)
    {
        var maxAllowed = TotalPrice.Amount * 1.2m;
        return newTotal.Amount > maxAllowed;
    }

    public override string ToString() => $"Materials: {MaterialsEstimate}, Labor: {LaborCost}, Total: {TotalPrice}";
}
