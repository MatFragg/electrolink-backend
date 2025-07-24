using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Components;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Components;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Events.Components;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class Component
{
    public ComponentId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public ComponentTypeId TypeId { get; private set; } 
    private readonly List<IEvent> _domainEvents = new(); // CAMBIO: Lista de eventos
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly(); 
    
    public Component() {}
    
    public Component(string name, string description, bool isActive, ComponentTypeId typeId) : this()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));
        
        if (typeId == null || typeId.Id <= 0)
            throw new ArgumentException("TypeId must be a valid ComponentTypeId.", nameof(typeId));
        
        Id = ComponentId.NewId();
        Name = name;
        Description = description;
        IsActive = isActive;
        TypeId = typeId;
        _domainEvents.Add(new ComponentCreatedEvent(Id, TypeId, Name, DateTime.UtcNow));

    }
    
    public Component(CreateComponentCommand command) : this(command.Name, command.Description,command.IsActive  , new ComponentTypeId(command.ComponentTypeId))
    {

    }

    public void UpdateInfo(UpdateComponentCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        TypeId = new ComponentTypeId(command.TypeId);
        IsActive = command.IsActive;

        _domainEvents.Add(new ComponentUpdatedEvent(
            Id,
            Name,
            Description, 
            TypeId.Id, 
            DateTime.UtcNow));
    }

    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
        _domainEvents.Add(new ComponentDeactivatedEvent(Id, DateTime.UtcNow)); 

    }
    
    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        _domainEvents.Add(new ComponentActivatedEvent(Id, DateTime.UtcNow)); 

    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}