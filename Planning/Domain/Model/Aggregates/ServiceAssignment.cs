using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

public class ServiceAssignment
{
    public ServiceId Id { get; private set; } = ServiceId.NewId();
    public RequestId RequestId { get; private set; }
    public TechnicianId TechnicianId { get; private set; }
    public HomeownerId HomeownerId { get; private set; }
    public PropertyId PropertyId { get; private set; }
    
    public RecipeSnapshot RecipeSnapshot { get; private set; } // inmutable
    public ScheduledSlot ScheduledSlot { get; private set; }
    public AssignmentStatus Status { get; private set; } = AssignmentStatus.Scheduled;
    public bool IsPriority { get; private set; }
    
    // Domain Events
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected ServiceAssignment() 
    {
        RequestId = new RequestId(Guid.Empty);
        TechnicianId = new TechnicianId(Guid.Empty);
        HomeownerId = new HomeownerId(Guid.Empty);
        PropertyId = new PropertyId(Guid.Empty);
        RecipeSnapshot = new RecipeSnapshot();
        ScheduledSlot = new ScheduledSlot();
    }
    
    // Factory method
    public static ServiceAssignment Create(
        RequestId requestId,
        TechnicianId technicianId,
        HomeownerId homeownerId,
        PropertyId propertyId,
        RecipeSnapshot recipeSnapshot,
        ScheduledSlot scheduledSlot,
        bool isPriority)
    {
        var assignment = new ServiceAssignment
        {
            RequestId = requestId,
            TechnicianId = technicianId,
            HomeownerId = homeownerId,
            PropertyId = propertyId,
            RecipeSnapshot = recipeSnapshot,
            ScheduledSlot = scheduledSlot,
            IsPriority = isPriority,
            Status = AssignmentStatus.Scheduled
        };
        
        assignment._domainEvents.Add(new ServiceAutomaticallyAssignedEvent(
            assignment.Id.Id,
            requestId.Id,
            technicianId.Id,
            homeownerId.Id,
            propertyId.Id,
            recipeSnapshot,
            scheduledSlot.StartDateTime,
            scheduledSlot.EndDateTime,
            isPriority,
            DateTime.UtcNow
        ));
        
        return assignment;
    }
    
    public void Cancel()
    {
        if (Status == AssignmentStatus.Cancelled)
            throw new InvalidOperationException("Assignment is already cancelled.");
        
        Status = AssignmentStatus.Cancelled;
        
        _domainEvents.Add(new ServiceAssignmentCancelledEvent(
            Id.Id,
            RequestId.Id,
            DateTime.UtcNow
        ));
    }
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}


