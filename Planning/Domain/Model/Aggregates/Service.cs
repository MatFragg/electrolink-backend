using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root representing a service in the catalog
/// Encapsulates business rules for service management
/// </summary>
public partial class Service
{
    public ServiceId Id { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money BasePrice { get; private set; } = null!;
    public string EstimatedTime { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public bool IsVisible { get; private set; }
    public TechnicianId CreatedBy { get; private set; } = null!;

    public ServicePolicy? Policy { get; private set; }
    public ServiceRestriction? Restriction { get; private set; }
    public ICollection<ServiceTag> Tags { get; private set; } = new List<ServiceTag>();
    public ICollection<ServiceComponent> Components { get; private set; } = new List<ServiceComponent>();
    public ICollection<ServicePlan> Plans { get; private set; } = new List<ServicePlan>();
    public ICollection<ServiceDocument> Documents { get; private set; } = new List<ServiceDocument>();

    // Domain Events
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Private constructor for EF Core
    /// </summary>
    private Service()
    {
    }

    /// <summary>
    /// Factory Method to create a new Service
    /// Validates business rules for service creation
    /// </summary>
    public static Service Create(
        string name,
        string description,
        Money basePrice,
        string estimatedTime,
        string category,
        TechnicianId createdBy,
        ServicePolicy? policy = null,
        ServiceRestriction? restriction = null,
        ICollection<ServiceTag>? tags = null,
        ICollection<ServiceComponent>? components = null)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Service name is required");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Service description is required");
        }

        if (basePrice == null || basePrice.Amount < 0)
        {
            throw new DomainException("Base price must be a positive value");
        }

        if (string.IsNullOrWhiteSpace(estimatedTime))
        {
            throw new DomainException("Estimated time is required");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new DomainException("Service category is required");
        }

        if (createdBy == null)
        {
            throw new DomainException("Creator technician ID is required");
        }

        var service = new Service
        {
            Id = ServiceId.NewId(),
            Name = name,
            Description = description,
            BasePrice = basePrice,
            EstimatedTime = estimatedTime,
            Category = category,
            CreatedBy = createdBy,
            IsVisible = true,
            Policy = policy,
            Restriction = restriction,
            Tags = tags ?? new List<ServiceTag>(),
            Components = components ?? new List<ServiceComponent>()
        };

        return service;
    }

    /// <summary>
    /// Adds a domain event to the aggregate
    /// </summary>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}