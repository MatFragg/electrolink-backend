using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class Component
{
    public ComponentId Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    
    public ComponentTypeId TypeId { get; private set; }

    public Component(CreateComponentCommand command) : this()
    {
        Id = ComponentId.NewId();
        Name = command.Name;
        Description = command.Description;
        TypeId = new ComponentTypeId(command.ComponentTypeId);
        IsActive = true;
    }

    public void UpdateInfo(UpdateComponentCommand command)
    {
        Name = command.Name;
        Description = command.Description;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    
    private Component() 
    {
        
        Name = string.Empty;
        Description = string.Empty;
        TypeId = new ComponentTypeId(0);
        IsActive = false;
    }
}