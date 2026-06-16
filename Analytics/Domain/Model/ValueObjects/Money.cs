namespace Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        if (amount < 0) throw new ArgumentException("Money amount cannot be negative.");
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency cannot be empty.");
        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Of(decimal amount, string currency) => new(amount, currency);
    public static Money Zero(string currency) => new(0, currency);
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money of different currencies.");
        return new Money(Amount + other.Amount, Currency);
    }
}
