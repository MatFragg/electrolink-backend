using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class TechnicianInventory
{
    // --- PROPIEDADES ---
    public Guid Id { get; private set; }
    public TechnicianId TechnicianId { get; private set; }

    private readonly List<ComponentStock> _stockItems;
    public IReadOnlyCollection<ComponentStock> StockItems => _stockItems.AsReadOnly();
    
    private TechnicianInventory()
    {
        _stockItems = new List<ComponentStock>();
        TechnicianId = null!;
    }

    public TechnicianInventory(CreateTechnicianInventoryCommand command) : this()
    {
        Id = Guid.NewGuid();
        TechnicianId = new TechnicianId(command.TechnicianId);
    }

    private ComponentStock? GetStockItemByComponentId(ComponentId componentId)
    {
        return _stockItems.FirstOrDefault(s => s.ComponentId == componentId);
    }

    public void Handle(AddStockToInventoryCommand command)
    {
        var componentId = new ComponentId(command.ComponentId);
        if (GetStockItemByComponentId(componentId) != null)
        {
            throw new InvalidOperationException($"Stock for component {componentId} already exists.");
        }
        var newStockItem = new ComponentStock(this.Id, componentId, command.Quantity, command.AlertThreshold);
        _stockItems.Add(newStockItem);
    }
    public void Handle(DecreaseStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(new(command.ComponentId)) 
            ?? throw new KeyNotFoundException("Component not found in inventory.");
        
        stockItem.DecreaseQuantity(command.AmountToDecrease);
    }
    
    public void Handle(IncreaseStockCommand command)
    {
        var stockItem = GetStockItemByComponentId(new(command.ComponentId)) 
            ?? throw new KeyNotFoundException("Component not found in inventory.");

        // La validación de la cantidad ahora está dentro del método IncreaseQuantity.
        stockItem.IncreaseQuantity(command.AmountToAdd);
    }
    
  
}