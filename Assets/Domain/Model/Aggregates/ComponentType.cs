using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class ComponentType
{
    public ComponentTypeId Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    private readonly List<IEvent> _domainEvents = new(); 
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();
    private ComponentType() 
    {
        Name = string.Empty;
        Description = string.Empty;
    }
    public ComponentType( ComponentTypeId id, string name, string description)
    {
        // Validaciones
        if (id == null || id.Id <= 0) throw new ArgumentException("Id must be a valid ComponentTypeId.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be null or empty.", nameof(description));
        
        Id = id; // El ID se pasa, generado en el Command Service (o por DB)
        Name = name;
        Description = description;

        // CAMBIO: Registrar evento de creación
        _domainEvents.Add(new ComponentTypeCreatedEvent(Id, Name, DateTime.UtcNow));
    }
    public ComponentType(CreateComponentTypeCommand command) : this(
        null!,
        command.Name, 
        command.Description)
    {
    }
    
    public void Update(UpdateComponentTypeCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        _domainEvents.Add(new ComponentTypeUpdatedEvent(Id, Name, Description, DateTime.UtcNow)); // Define ComponentTypeUpdatedEvent

    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}