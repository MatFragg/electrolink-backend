using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Entities;

public class ReservationItem
{
    public ReservationItemId Id { get; private set; } = null!;
    public ComponentReservationId ReservationId { get; private set; } = null!;
    public ComponentId ComponentId { get; private set; } = null!;
    public ComponentTypeId ComponentTypeId { get; private set; } = null!;
    public int Quantity { get; private set; }

    private ReservationItem() { }

    public static ReservationItem Create(
        ComponentReservationId reservationId,
        ComponentId componentId, 
        ComponentTypeId componentTypeId,
        int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        return new ReservationItem
        {
            Id = ReservationItemId.NewReservationItemId(),
            ReservationId = reservationId,
            ComponentId = componentId,
            ComponentTypeId = componentTypeId,
            Quantity = quantity
        };
    }
}