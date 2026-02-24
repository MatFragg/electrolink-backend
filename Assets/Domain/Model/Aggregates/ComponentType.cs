using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class ComponentType : BaseAggregateRoot
{
    public ComponentTypeId Id { get; init; }
    [Column(TypeName = "varchar(100)")]
    public string Name { get; private set; } = string.Empty;
    [Column(TypeName = "varchar(500)")]
    public string Description { get; private set; } = string.Empty;
    
    public ComponentType() 
    {
    }
    public ComponentType(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be null or empty.", nameof(description));
        
        Name = name;
        Description = description;

    }
    public ComponentType(CreateComponentTypeCommand command) : this(
        command.Name, 
        command.Description) {
    }
    
    public void Update(UpdateComponentTypeCommand command)
    {
        Name = command.Name;
        Description = command.Description;
    }
}