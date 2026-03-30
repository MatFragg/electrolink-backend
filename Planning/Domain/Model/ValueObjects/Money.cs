using System.Text.Json.Serialization;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record Money
{
    public decimal  Amount   { get; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ECurrency Currency { get; }

    private Money() { }
    
    [JsonConstructor]
    public Money(decimal amount, ECurrency currency)
    {
        if (amount < 0) throw new InvalidMoneyException("El monto no puede ser negativo.");
        Amount = Math.Round(amount, 2);
        Currency = currency;
    }

    public static Money Of(decimal amount, ECurrency currency)
        => new(amount, currency);

    public Money Add(Money other)
    {
        AssertSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public bool IsLessThan(Money other)
    {
        AssertSameCurrency(other);
        return Amount < other.Amount;
    }

    private void AssertSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new CurrencyMismatchException(Currency, other.Currency);
    }

    public override string ToString() => $"{Amount:F2} {Currency}";
}