using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

public class Plan
{
    public PlanId Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; }
    public MonetizationType MonetizationType { get; private set; }
    public bool IsDefault { get; private set; }

    private Plan() {}

    public Plan(string name, string description, decimal price, string currency, MonetizationType monetizationType, bool isDefault = false)
    {
        Id = new PlanId(Guid.NewGuid());
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
        MonetizationType = monetizationType;
        IsDefault = isDefault;
    }

    public void MarkAsDefault() => IsDefault = true;
    public void UnmarkAsDefault() => IsDefault = false;
    
    public void UpdateDetails(string name, string description, decimal price, string currency, MonetizationType monetizationType, bool isDefault)
    {
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
        MonetizationType = monetizationType;
        IsDefault = isDefault;
    }
}