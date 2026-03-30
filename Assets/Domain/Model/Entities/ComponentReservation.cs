using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;

public class ComponentReservation
{
    public ComponentReservationId Id { get; private set; } = null!;
    public TechnicianInventoryId TechnicianInventoryId { get; private set; } = null!;
    public AssignmentId AssignmentId { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsConsumed { get; private set; }
    public bool IsReleased { get; private set; }
    public DateTime? ConsumedAt { get; private set; }
    public DateTime? ReleasedAt { get; private set; }

    private readonly List<ReservationItem> _items = new();
    public IReadOnlyCollection<ReservationItem> Items => _items.AsReadOnly();

    private ComponentReservation() { }
    
    public static ComponentReservation Create(TechnicianInventoryId inventoryId, AssignmentId assignmentId, DateTime expiresAt)
    {
        return new ComponentReservation
        {
            Id = ComponentReservationId.NewComponentReservationId(),
            TechnicianInventoryId = inventoryId,
            AssignmentId   = assignmentId,
            ExpiresAt   = expiresAt,
            IsConsumed  = false,
            IsReleased  = false,
        };
    }

    public bool IsExpired => !IsConsumed && !IsReleased && DateTime.UtcNow > ExpiresAt;

    internal void AddItem(ComponentId componentId, ComponentTypeId componentTypeId, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        _items.Add(ReservationItem.Create(Id, componentId, componentTypeId, quantity));
    }

    internal void MarkAsConsumed()
    {
        IsConsumed = true;
        ConsumedAt = DateTime.UtcNow;
    }

    internal void Release()
    {
        IsReleased = true;
        ReleasedAt = DateTime.UtcNow;
    }
}