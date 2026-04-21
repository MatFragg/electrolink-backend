namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
/// Value Object representing a monetary amount with currency.
/// Amount is stored in cents (e.g., 2900 = $29.00).
/// </summary>
public record Money
{
    /// <summary>
    /// Amount in cents (e.g., 2900 = $29.00).
    /// </summary>
    public int Amount { get; }

    /// <summary>
    /// ISO 4217 currency code (e.g., "usd", "pen").
    /// </summary>
    public string Currency { get; }

    private Money(int amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");
        
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty.");

        Amount = amount;
        Currency = currency.ToLowerInvariant();
    }

    /// <summary>
    /// Factory method to create a Money instance.
    /// </summary>
    public static Money Of(int amount, string currency) => new(amount, currency);

    /// <summary>
    /// Human-readable string representation (e.g., "$29.00 USD").
    /// </summary>
    public override string ToString() => $"{Amount / 100.0:F2} {Currency.ToUpperInvariant()}";
}

