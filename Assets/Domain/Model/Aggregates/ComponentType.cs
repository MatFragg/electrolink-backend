using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.ComponentTypes;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class ComponentType
{
    public ComponentTypeId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    
    public ComponentType() {
        
    }
    
    public ComponentType( ComponentTypeId id,string name, string description) : this()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));
        
        Id = id;
        Name = name;
        Description = description;
    }
    

    public ComponentType(CreateComponentTypeCommand command)
    {
        // Genera una nueva ID para esta nueva entidad
          
        Name = command.Name;
        Description = command.Description;

        // Es crucial que las validaciones que tenías en el otro constructor también se apliquen aquí
        // o que se hagan en un método privado común.
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(Name));
        
        if (string.IsNullOrWhiteSpace(Description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(Description));
    }
    
    public void Update(UpdateComponentTypeCommand command)
    {
        Name = command.Name;
        Description = command.Description;
    }
    
}