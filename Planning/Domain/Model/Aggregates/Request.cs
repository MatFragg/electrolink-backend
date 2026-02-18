using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

/// <summary>
/// Aggregate Root representing a service request
/// Encapsulates business rules for request lifecycle management
/// </summary>
public partial class Request
{
    // Identity and relationships
    public RequestId Id { get; private set; } = null!;
    public ClientId ClientId { get; private set; } = null!;
    public TechnicianId? TechnicianId { get; private set; }
    public PropertyId PropertyId { get; private set; } = null!;
    public ServiceId ServiceId { get; private set; } = null!;

    // State and behavior
    public RequestStatus Status { get; private set; } = null!;
    public RequestPriority Priority { get; private set; } = null!;
    public DateOnly ScheduledDate { get; private set; }
    public string ProblemDescription { get; private set; } = string.Empty;

    // Collections
    public List<RequestPhoto> Photos { get; private set; } = new();
    public ElectricBill? Bill { get; private set; }

    // Domain Events
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Private constructor for EF Core
    /// </summary>
    private Request()
    {
    }

    /// <summary>
    /// Factory Method to create a new Request
    /// Validates business rules including subscription plan limits
    /// </summary>
    public static Request Create(
        ClientId clientId,
        PropertyId propertyId,
        ServiceId serviceId,
        DateOnly scheduledDate,
        string problemDescription,
        bool isPremiumUser,
        int currentMonthUsage,
        ElectricBill? bill = null)
    {
        // Business Rule: Basic plan users are limited to 2 requests per month
        if (!isPremiumUser && currentMonthUsage >= 2)
        {
            throw new MonthlyRequestLimitExceededException(2, currentMonthUsage);
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(problemDescription))
        {
            throw new DomainException("Problem description is required");
        }

        if (scheduledDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new DomainException("Scheduled date cannot be in the past");
        }

        var request = new Request
        {
            Id = RequestId.NewId(),
            ClientId = clientId,
            PropertyId = propertyId,
            ServiceId = serviceId,
            Status = RequestStatus.Pending,
            Priority = RequestPriority.FromPremiumStatus(isPremiumUser),
            ScheduledDate = scheduledDate,
            ProblemDescription = problemDescription,
            TechnicianId = null, // Will be assigned automatically via event handler
            Bill = bill
        };

        // Raise domain event to trigger automatic technician assignment
        request.AddDomainEvent(new RequestCreatedEvent(
            request.Id,
            request.ClientId,
            request.PropertyId,
            request.ServiceId,
            request.Priority,
            request.ScheduledDate,
            DateTime.UtcNow
        ));

        return request;
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