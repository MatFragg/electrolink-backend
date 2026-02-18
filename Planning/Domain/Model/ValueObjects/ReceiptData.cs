namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ReceiptData
{
    public decimal ConsumptionKwh { get; init; }
    public Money AmountPaid { get; init; }
    public string BillingPeriod { get; init; }
    public string ReceiptNumber { get; init; }
    
    public ReceiptData() : this(0, Money.Zero(), string.Empty, string.Empty) { }
    
    public ReceiptData(decimal consumptionKwh, Money amountPaid, string billingPeriod, string receiptNumber)
    {
        if (consumptionKwh <= 0)
            throw new ArgumentException("Consumption must be > 0");
        if (string.IsNullOrWhiteSpace(billingPeriod) || !System.Text.RegularExpressions.Regex.IsMatch(billingPeriod, @"^\d{4}-\d{2}$"))
            throw new ArgumentException("Billing period must be in YYYY-MM format");
            
        ConsumptionKwh = consumptionKwh;
        AmountPaid = amountPaid ?? Money.Zero();
        BillingPeriod = billingPeriod;
        ReceiptNumber = receiptNumber ?? string.Empty;
    }
}

