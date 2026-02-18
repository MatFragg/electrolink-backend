using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;

public class ServiceRecipe
{
    public RecipeId Id { get; private set; } = RecipeId.NewId();
    public CatalogId CatalogId { get; private set; }
    public ServiceName ServiceName { get; private set; }
    public string ServiceDescription { get; private set; }
    public ServiceCategory ServiceCategory { get; private set; } // inmutable
    public Pricing Pricing { get; private set; }
    public EstimatedDuration EstimatedDuration { get; private set; }
    public WarrantyPeriod WarrantyPeriod { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int TimesRequested { get; private set; } = 0;
    public string? DeactivationReason { get; private set; }
    
    // Collections
    private readonly List<ComponentRequirement> _componentRequirements = new();
    public IReadOnlyCollection<ComponentRequirement> ComponentRequirements => _componentRequirements.AsReadOnly();
    
    private readonly List<string> _prerequisites = new();
    public IReadOnlyCollection<string> Prerequisites => _prerequisites.AsReadOnly();
    
    private readonly List<string> _deliverables = new();
    public IReadOnlyCollection<string> Deliverables => _deliverables.AsReadOnly();
    
    // Constructor para EF Core
    protected ServiceRecipe() 
    {
        ServiceName = new ServiceName();
        ServiceDescription = string.Empty;
        Pricing = new Pricing();
        EstimatedDuration = new EstimatedDuration();
        WarrantyPeriod = new WarrantyPeriod();
        CatalogId = new CatalogId();
    }
    
    // Constructor principal
    public ServiceRecipe(CreateServiceRecipeCommand command)
    {
        ServiceName = new ServiceName(command.ServiceName);
        ServiceDescription = command.ServiceDescription;
        ServiceCategory = command.ServiceCategory;
        Pricing = new Pricing(
            new Money(command.MaterialsEstimate, command.Currency),
            new Money(command.LaborCost, command.Currency),
            new Money(command.TotalPrice, command.Currency)
        );
        EstimatedDuration = new EstimatedDuration(command.EstimatedHours, command.EstimatedMinutes);
        WarrantyPeriod = new WarrantyPeriod(command.WarrantyValue, command.WarrantyUnit);
        CatalogId = new CatalogId(command.CatalogId);
        
        foreach (var req in command.ComponentRequirements)
            _componentRequirements.Add(new ComponentRequirement(
                req.ComponentTypeId, req.ComponentTypeName, req.Quantity, req.IsRequired));
        
        _prerequisites.AddRange(command.Prerequisites ?? new List<string>());
        _deliverables.AddRange(command.Deliverables ?? new List<string>());
    }
    
    public void Update(UpdateServiceRecipeCommand command)
    {
        // Validar restricciones si hay servicios activos (se valida en command service)
        ServiceName = new ServiceName(command.ServiceName);
        ServiceDescription = command.ServiceDescription;
        Pricing = new Pricing(
            new Money(command.MaterialsEstimate, command.Currency),
            new Money(command.LaborCost, command.Currency),
            new Money(command.TotalPrice, command.Currency)
        );
        EstimatedDuration = new EstimatedDuration(command.EstimatedHours, command.EstimatedMinutes);
        WarrantyPeriod = new WarrantyPeriod(command.WarrantyValue, command.WarrantyUnit);
        
        _componentRequirements.Clear();
        foreach (var req in command.ComponentRequirements)
            _componentRequirements.Add(new ComponentRequirement(
                req.ComponentTypeId, req.ComponentTypeName, req.Quantity, req.IsRequired));
        
        _prerequisites.Clear();
        _prerequisites.AddRange(command.Prerequisites ?? new List<string>());
        
        _deliverables.Clear();
        _deliverables.AddRange(command.Deliverables ?? new List<string>());
    }
    
    public void Deactivate(string reason)
    {
        IsActive = false;
        DeactivationReason = reason;
    }
    
    public void Reactivate()
    {
        IsActive = true;
        DeactivationReason = null;
    }
    
    public void IncrementTimesRequested() => TimesRequested++;
    
    public RecipeSnapshot ToSnapshot() => new RecipeSnapshot(
        Id.Id,
        ServiceName.Value,
        ServiceCategory,
        ComponentRequirements.ToList(),
        Pricing,
        EstimatedDuration,
        WarrantyPeriod,
        DateTime.UtcNow
    );
}

