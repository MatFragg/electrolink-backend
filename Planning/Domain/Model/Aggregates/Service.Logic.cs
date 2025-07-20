using Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.Aggregates;

public partial class Service
{
    public void Update(
        string name,
        string description,
        double basePrice,
        string estimatedTime,
        string category,
        ServicePolicy policy,
        ServiceRestriction restriction,
        ICollection<ServiceTag> tags,
        ICollection<ServiceComponent> components)
    {
        Name = name;
        Description = description;
        BasePrice = basePrice;
        EstimatedTime = estimatedTime;
        Category = category;
        Policy = policy;
        Restriction = restriction;
        Tags = tags;
        Components = components;
    }

    public void Hide() => IsVisible = false;

    public void Show() => IsVisible = true;

    public void AddComponent(ServiceComponent component)
    {
        if (!Components.Any(c => c.ComponentId == component.ComponentId))
            Components.Add(component);
    }

    public void RemoveComponent(string componentId)
    {
        var item = Components.FirstOrDefault(c => c.ComponentId == componentId);
        if (item != null) Components.Remove(item);
    }

    public void AddTag(ServiceTag tag)
    {
        if (!Tags.Any(t => t.Name == tag.Name))
            Tags.Add(tag);
    }

    public void RemoveTag(string tagName)
    {
        var tag = Tags.FirstOrDefault(t => t.Name == tagName);
        if (tag != null) Tags.Remove(tag);
    }
}