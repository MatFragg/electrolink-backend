using System.ComponentModel.DataAnnotations.Schema;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class ComponentType : BaseAggregateRoot
{
    public ComponentTypeId Id { get; init; } = null!;
    [Column(TypeName = "varchar(100)")]
    public string Name { get; private set; } = string.Empty;
    [Column(TypeName = "varchar(500)")]
    public string Description { get; private set; } = string.Empty;
    
    public ComponentType() 
    {
    }
    public static ComponentType Create(string name, string description)
    => new ComponentType {
        Id = ComponentTypeId.NewComponentTypeId(),
        Name = name, 
        Description = description
    };
        
    public void Update(UpdateComponentTypeCommand command)
    {
        Name = command.Name;
        Description = command.Description ?? string.Empty;
    }
}