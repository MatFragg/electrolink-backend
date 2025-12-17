using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
/// Represents a subscription plan that users can subscribe to. This is the root aggregate for managing plans.
/// </summary>
public class Plan
{
    /// <summary>
    /// Unique identifier for the plan.
    /// </summary>
    public PlanId Id { get; private set; }

    /// <summary>
    /// The name of the plan (e.g., "Freemium", "Premium Propietario").
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// A detailed description of the plan.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// The price of the plan.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// The currency of the plan's price (e.g., "USD").
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    /// The monetization type (e.g., Free, Monthly, Annually).
    /// </summary>
    public EMonetizationType MonetizationType { get; private set; }

    /// <summary>
    /// Indicates if this is a default plan (e.g., the default Freemium plan).
    /// </summary>
    public bool IsDefault { get; private set; }

    /// <summary>
    /// The target role for this plan (Homeowner, Technician, or All).
    /// </summary>
    public EUserRole TargetRole { get; private set; }

    /// <summary>
    /// A collection of benefits included in this plan.
    /// </summary>
    public List<Benefit> Benefits { get; private set; } = new List<Benefit>();
    
    /// <summary>
    /// The ID of this plan in Stripe (Price ID). Can be null for free plans.
    /// </summary>
    public PaymentGatewayPriceId? GatewayPriceId  { get; private set; }
    
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Private constructor for ORM or deserialization.
    /// </summary>
    private Plan() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Plan"/> class.
    /// </summary>
    /// <param name="name">The name of the plan.</param>
    /// <param name="description">The description of the plan.</param>
    /// <param name="price">The price of the plan.</param>
    /// <param name="currency">The currency.</param>
    /// <param name="monetizationType">The monetization type.</param>
    /// <param name="targetRole">The target user role for this plan.</param>
    /// <param name="isDefault">Indicates if it's a default plan.</param>
    /// <param name="benefits">The list of benefits.</param>
    /// <param name="gatewayPriceId">Optional Stripe Price ID associated with this plan.</param>
    public Plan(string name, string description, decimal price, string currency, EMonetizationType monetizationType, EUserRole targetRole, bool isDefault, List<Benefit> benefits, PaymentGatewayPriceId? gatewayPriceId)
    {
        Id = new PlanId(Guid.NewGuid());
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
        MonetizationType = monetizationType;
        IsDefault = isDefault;
        TargetRole = targetRole;
        Benefits = benefits ?? new List<Benefit>();
        GatewayPriceId = gatewayPriceId;
        
        _domainEvents.Add(new PlanCreatedEvent(
            Id.Value,
            Name,
            Price,
            MonetizationType,
            TargetRole,
            IsDefault,
            Benefits,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Updates the details of the plan.
    /// </summary>
    /// <param name="name">The new name.</param>
    /// <param name="description">The new description.</param>
    /// <param name="price">The new price.</param>
    /// <param name="currency">The new currency.</param>
    /// <param name="monetizationType">The new monetization type.</param>
    /// <param name="targetRole">The new target user role.</param>
    /// <param name="isDefault">The new default status.</param>
    /// <param name="benefits">The updated list of benefits.</param>
    /// <param name="gatewayPriceId">Optional new Stripe Price ID associated with this plan.</param>
    public void UpdateDetails(string name, string description, decimal price, string currency, EMonetizationType monetizationType, EUserRole targetRole, bool isDefault, List<Benefit> benefits, PaymentGatewayPriceId? gatewayPriceId = null)
    {
        if (Name == name && Description == description && Price == price && Currency == currency &&
            MonetizationType == monetizationType && IsDefault == isDefault && TargetRole == targetRole &&
            GatewayPriceId == gatewayPriceId &&
            Benefits.SequenceEqual(benefits ?? new List<Benefit>()))
        {
            return; 
        }
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
        MonetizationType = monetizationType;
        IsDefault = isDefault;
        TargetRole = targetRole;
        Benefits = benefits ?? new List<Benefit>();
        GatewayPriceId = gatewayPriceId;
        
        _domainEvents.Add(new PlanDetailsUpdatedEvent(
            Id.Value, 
            name, 
            description, 
            price, 
            currency, 
            monetizationType, 
            isDefault, 
            targetRole,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Adds a benefit to the plan.
    /// </summary>
    /// <param name="benefit">The benefit to add.</param>
    public void AddBenefit(Benefit benefit)
    {
        Benefits.Add(benefit);
    }

    /// <summary>
    /// Removes a benefit from the plan.
    /// </summary>
    /// <param name="benefitType">The type of the benefit to remove.</param>
    public void RemoveBenefit(string benefitType)
    {
        Benefits.RemoveAll(b => b.Type == benefitType);
    }

    /// <summary>
    /// Checks if the plan has a specific benefit.
    /// </summary>
    /// <param name="benefitType">The type of benefit to check.</param>
    /// <returns>True if the plan has the benefit, false otherwise.</returns>
    public bool HasBenefit(string benefitType)
    {
        return Benefits.Exists(b => b.Type == benefitType);
    }

    /// <summary>
    /// Gets a specific benefit from the plan.
    /// </summary>
    /// <param name="benefitType">The type of benefit to retrieve.</param>
    /// <returns>The <see cref="Benefit"/> if found, otherwise null.</returns>
    public Benefit? GetBenefit(string benefitType)
    {
        if (string.IsNullOrEmpty(benefitType))
            return null;

        return Benefits.Find(b => b.Type == benefitType);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}