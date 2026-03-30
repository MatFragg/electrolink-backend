using System.Text.Json.Serialization;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record WarrantyPeriod
{
    public int Months { get; }

    private WarrantyPeriod() { }

    [JsonConstructor]
    public WarrantyPeriod(int months)
    {
        Months = months;
    }

    public static WarrantyPeriod OfMonths(int months) {
        if (months < 0)
            throw new InvalidWarrantyPeriodException("El período de garantía no puede ser negativo.");
        return new WarrantyPeriod(months);
    }
}
