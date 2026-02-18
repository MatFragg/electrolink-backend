using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;

public partial class ServiceCatalog
{
    public CatalogId Id { get; private set; } = CatalogId.NewId();
    public TechnicianId TechnicianId { get; private set; }
    public CatalogStatus Status { get; private set; } = CatalogStatus.Empty;
    
    // Collection de recipes (entidades internas)
    private readonly List<ServiceRecipe> _recipes = new();
    public IReadOnlyCollection<ServiceRecipe> Recipes => _recipes.AsReadOnly();
    
    // Domain Events
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    // Constructor para EF Core
    protected ServiceCatalog() 
    {
        TechnicianId = new TechnicianId(Guid.Empty);
    }
    
    // Constructor principal
    public ServiceCatalog(TechnicianId technicianId)
    {
        if (technicianId == null || technicianId.Id == Guid.Empty)
            throw new ArgumentException("Technician ID must be valid.", nameof(technicianId));
        
        TechnicianId = technicianId;
        Status = CatalogStatus.Empty;
        
        _domainEvents.Add(new ServiceCatalogCreatedEvent(
            Id.Id,
            technicianId.Id,
            DateTime.UtcNow
        ));
    }
    
    // Constructor desde comando
    public ServiceCatalog(CreateServiceCatalogCommand command) 
        : this(command.TechnicianId) { }
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}


