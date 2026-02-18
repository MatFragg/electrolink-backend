using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

public partial class ServiceRequest
{
    public RequestId Id { get; private set; } = RequestId.NewId();
    public HomeownerId HomeownerId { get; private set; }
    public RequestStatus Status { get; private set; } = RequestStatus.Draft;
    public bool IsPriority { get; private set; } = false;
    
    // Wizard step 2: Property selection
    public PropertySnapshot? PropertySnapshot { get; private set; }
    
    // Wizard step 3-4: Service selection
    public RecipeId? SelectedRecipeId { get; private set; }
    public TechnicianId? SelectedTechnicianId { get; private set; }
    public ReceiptData? ReceiptData { get; private set; }
    public RequestPreferences? Preferences { get; private set; }
    
    // Post-assignment
    public ServiceId? AssignedServiceId { get; private set; }
    public string? CancellationReason { get; private set; }
    
    // Domain Events
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected ServiceRequest() 
    {
        HomeownerId = new HomeownerId(Guid.Empty);
    }
    
    // Factory method - Wizard Step 1: Initiate
    public static ServiceRequest Initiate(HomeownerId homeownerId)
    {
        if (homeownerId == null || homeownerId.Id == Guid.Empty)
            throw new ArgumentException("Homeowner ID must be valid.", nameof(homeownerId));
        
        var request = new ServiceRequest
        {
            HomeownerId = homeownerId,
            Status = RequestStatus.Draft
        };
        
        request._domainEvents.Add(new ServiceRequestInitiatedEvent(
            request.Id.Id,
            homeownerId.Id,
            DateTime.UtcNow
        ));
        
        return request;
    }
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}


