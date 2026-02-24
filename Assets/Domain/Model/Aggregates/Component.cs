using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class Component : BaseAggregateRoot
{
    public ComponentId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public ComponentTypeId TypeId { get; private set; } 
    
    private Component() {}
    
    public static Component Create(string name, string description, bool isActive, ComponentTypeId typeId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));
        
        if (typeId == null || typeId.Value == string.Empty)
            throw new ArgumentException("TypeId must be a valid ComponentTypeId.", nameof(typeId));

        var component = new Component
        {
            Id = ComponentId.NewComponentId(),
            Name = name,
            Description = description,
            IsActive = isActive,
            TypeId = typeId
        };
        
        
        return component;
    }
    
    public void UpdateInfo(UpdateComponentCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        TypeId = ComponentTypeId.From(command.ComponentTypeId);
        IsActive = command.IsActive;

    }

    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
    }
    
    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
    }
}