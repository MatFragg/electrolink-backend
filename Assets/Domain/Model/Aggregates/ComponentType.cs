using System.ComponentModel.DataAnnotations.Schema;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class ComponentType : BaseAggregateRoot
{
    public ComponentTypeId Id { get; init; } = null!;
    [Column(TypeName = "varchar(100)")] public string Name { get; private set; } = string.Empty;
    [Column(TypeName = "varchar(500)")] public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public ComponentType()
    {
    }

    public static ComponentType Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));

        var componentType = new ComponentType {
            Id = ComponentTypeId.NewComponentTypeId(),
            Name = name,
            Description = description,
            IsActive = true
        };
        
        componentType.RaiseDomainEvent(new ComponentTypeCreatedEvent(
            componentType.Id, componentType.Name, DateTime.UtcNow));
        
        return componentType;
    }

    public void Update(UpdateComponentTypeCommand command) 
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name cannot be empty.", nameof(command.Name));
        
        Name = command.Name;
        Description = command.Description ?? string.Empty;
        
        RaiseDomainEvent(new ComponentTypeUpdatedEvent(Id, Name, Description, DateTime.UtcNow));

    }
    
    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
    }
}