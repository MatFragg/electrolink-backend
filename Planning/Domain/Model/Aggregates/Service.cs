using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;

public partial class Service
{
    public string ServiceId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public double BasePrice { get; private set; }
    public string EstimatedTime { get; private set; }
    public string Category { get; private set; }
    public bool IsVisible { get; private set; }
    public string CreatedBy { get; private set; }

    public ServicePolicy Policy { get; private set; }
    public ServiceRestriction Restriction { get; private set; }
    public ICollection<ServiceTag> Tags { get; private set; } = new List<ServiceTag>();
    public ICollection<ServiceComponent> Components { get; private set; } = new List<ServiceComponent>();
    public ICollection<ServicePlan> Plans { get; private set; } = new List<ServicePlan>();
    public ICollection<ServiceDocument> Documents { get; private set; } = new List<ServiceDocument>();

    public Service()
    {
        Name = string.Empty;
        Description = string.Empty;
        Category = string.Empty;
        CreatedBy = string.Empty;
    }

    public Service(CreateServiceCommand command) : this()
    {
        ServiceId = command.ServiceId;
        Name = command.Name;
        Description = command.Description;
        BasePrice = command.BasePrice;
        EstimatedTime = command.EstimatedTime;
        Category = command.Category;
        CreatedBy = command.CreatedBy;
        IsVisible = true;
        Policy = command.Policy;
        Restriction = command.Restriction;
        Tags = command.Tags;
        Components = command.Components;
    }
    
    public void UpdateDetails(string name, string description, double basePrice, string estimatedTime, string category, bool isVisible)
    {
        Name = name;
        Description = description;
        BasePrice = basePrice;
        EstimatedTime = estimatedTime;
        Category = category;
        IsVisible = isVisible;
    }
}