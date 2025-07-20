using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class ComponentType
{
    public ComponentTypeId Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    private ComponentType() 
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public ComponentType(CreateComponentTypeCommand command) : this()
    {
        Name = command.Name;
        Description = command.Description;
    }
    
    public void Update(UpdateComponentTypeCommand command)
    {
        Name = command.Name;
        Description = command.Description;
    }
    
}