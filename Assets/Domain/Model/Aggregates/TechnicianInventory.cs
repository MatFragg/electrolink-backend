using Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Exceptions;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class TechnicianInventory : BaseAggregateRoot
{
    public TechnicianInventoryId Id { get; private set; } = null!;
    public TechnicianId TechnicianId { get; private set; } = null!;
    public EInventoryStatus Status { get; private set; }
    private readonly List<ComponentStock> _stockItems   = new();
    private readonly List<ComponentReservation> _reservations = new();
    public IReadOnlyCollection<ComponentStock> StockItems => _stockItems.AsReadOnly();
    public IReadOnlyCollection<ComponentReservation> Reservations => _reservations.AsReadOnly();

    private TechnicianInventory() {}

    public static TechnicianInventory Create(TechnicianId technicianId)
    {
        var inventory = new TechnicianInventory
        {
            Id           = TechnicianInventoryId.NewTechnicianInventoryId(),
            TechnicianId = technicianId,
            Status       = EInventoryStatus.Empty,
        };

        inventory.RaiseDomainEvent(new TechnicianInventoryCreatedEvent(
            inventory.Id, inventory.TechnicianId, DateTime.UtcNow));

        return inventory;
    }

    internal void AddStock(ComponentId componentId, ComponentTypeId componentTypeId, int quantity, int alertThreshold)
    {
        if (FindStockByComponentId(componentId) is not null)
            throw new InvalidOperationException($"Stock for component {componentId.Value} already exists.");

        var stock = ComponentStock.Create(Id, componentId, componentTypeId, quantity, alertThreshold);
        _stockItems.Add(stock);

        if (Status == EInventoryStatus.Empty)
            Status = EInventoryStatus.Active;
    }

    internal void IncreaseStock(ComponentId componentId, int amount)
    {
        var stock = FindStockOrThrow(componentId);
        stock.IncreaseQuantity(amount);

        RaiseDomainEvent(new ComponentStockIncreasedEvent(
            stock.Id, stock.ComponentId, amount, stock.QuantityAvailable, DateTime.UtcNow));
    }
    
    internal void DecreaseStock(ComponentId componentId, int amount)
    {
        var stock = FindStockOrThrow(componentId);
        stock.DecreaseQuantity(amount);

        RaiseDomainEvent(new ComponentStockDecreasedEvent(
            stock.Id, stock.ComponentId, amount, stock.QuantityAvailable, DateTime.UtcNow));

        if (stock.QuantityAvailable <= stock.AlertThreshold)
            RaiseDomainEvent(new ComponentStockLowEvent(
                stock.Id, stock.ComponentId, stock.QuantityAvailable, stock.AlertThreshold, DateTime.UtcNow));
    }
    
    internal void UpdateStock(ComponentId componentId, int newQuantity, int newAlertThreshold)
    {
        var stock = FindStockOrThrow(componentId);
        int difference = newQuantity - stock.QuantityAvailable;

        if (difference > 0)
        {
            stock.IncreaseQuantity(difference);
            RaiseDomainEvent(new ComponentStockIncreasedEvent(
                stock.Id, stock.ComponentId, difference, stock.QuantityAvailable, DateTime.UtcNow));
        }
        else if (difference < 0)
        {
            stock.DecreaseQuantity(-difference);
            RaiseDomainEvent(new ComponentStockDecreasedEvent(
                stock.Id, stock.ComponentId, -difference, stock.QuantityAvailable, DateTime.UtcNow));
        }

        if (stock.AlertThreshold != newAlertThreshold)
        {
            stock.UpdateAlertThreshold(newAlertThreshold);
            RaiseDomainEvent(new ComponentStockThresholdUpdatedEvent(
                stock.Id, stock.ComponentId, stock.AlertThreshold, DateTime.UtcNow));
        }

        if (stock.QuantityAvailable <= stock.AlertThreshold)
            RaiseDomainEvent(new ComponentStockLowEvent(
                stock.Id, stock.ComponentId, stock.QuantityAvailable, stock.AlertThreshold, DateTime.UtcNow));
    }
    
    internal void RemoveStock(ComponentId componentId)
    {
        var stock = FindStockOrThrow(componentId);

        if (stock.QuantityAvailable > 0)
            throw new InvalidOperationException(
                $"Cannot remove component {componentId.Value} while stock is greater than 0.");

        _stockItems.Remove(stock);
    }
    
    internal void ReserveComponentsForService(
        AssignmentId assignmentId,
        IReadOnlyList<ComponentAdjustment> items)
    {
        foreach (var item in items)
        {
            var stock = FindStockOrThrow(item.ComponentId);
            if (stock.AvailableForReservation < item.Quantity)
                throw new InsufficientStockException(item.ComponentId, stock.AvailableForReservation, item.Quantity);
        }

        var reservation = ComponentReservation.Create(Id, assignmentId, DateTime.UtcNow.AddHours(72));

        foreach (var item in items)
        {
            var stock = FindStockOrThrow(item.ComponentId); 
            reservation.AddItem(item.ComponentId, stock.ComponentTypeId, item.Quantity);
        }
            
        _reservations.Add(reservation);

        foreach (var item in items)
            FindStockByComponentId(item.ComponentId)!.Reserve(item.Quantity);

        RaiseDomainEvent(new ComponentsReservedForServiceEvent(
            Id, assignmentId, items, reservation.ExpiresAt, DateTime.UtcNow));
    }
    
    internal void ConsumeComponentsForService(AssignmentId assignmentId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.AssignmentId == assignmentId) ?? throw new ReservationNotFoundException(assignmentId);

        foreach (var item in reservation.Items)
            FindStockOrThrow(item.ComponentId).Consume(item.Quantity);

        reservation.MarkAsConsumed();

        RaiseDomainEvent(
            new ComponentsConsumedEvent(
                Id, 
                assignmentId, 
                reservation.Items.Select(i => new ComponentAdjustment(
                    i.ComponentId, 
                    i.Quantity)), 
                DateTime.UtcNow));
    }
    
    internal void ReleaseReservation(AssignmentId assignmentId, string reason)
    {
        var reservation = _reservations.FirstOrDefault(r => r.AssignmentId == assignmentId) ?? throw new ReservationNotFoundException(assignmentId);

        foreach (var item in reservation.Items)
            FindStockByComponentId(item.ComponentId)?.Release(item.Quantity);

        reservation.Release();

        RaiseDomainEvent(new ComponentReservationReleasedEvent(
            Id, assignmentId, reason, DateTime.UtcNow));
    }
    
    internal void AdjustComponentQuantity(ComponentId componentId, int quantityAdjustment)
    {
        var item = _stockItems.FirstOrDefault(s => s.ComponentId == componentId);

        if (item is not null)
        {
            item.UpdateQuantity(item.QuantityAvailable + quantityAdjustment);
            if (item.QuantityAvailable <= 0)
                _stockItems.Remove(item);
        }
        
        /*
        else if (quantityAdjustment > 0)
        {
            _stockItems.Add(ComponentStock.Create(Id, componentId, componentTypeId, quantityAdjustment, 0));
        }*/
    }
    
    private ComponentStock? FindStockByComponentId(ComponentId id)
        => _stockItems.FirstOrDefault(s => s.ComponentId == id);

    private ComponentStock FindStockOrThrow(ComponentId id)
        => FindStockByComponentId(id)
           ?? throw new KeyNotFoundException($"Component {id.Value} not found in inventory.");
}