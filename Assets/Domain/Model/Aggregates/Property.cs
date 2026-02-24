using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using PropertyId = Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects.PropertyId;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class Property : BaseAggregateRoot
{
    public PropertyId Id { get; private set; } = null!;
    public HomeownerId OwnerId { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public Geolocation Geolocation { get; private set; } = null!;
    public Region Region { get; private set; } = null!;
    public District District { get; private set; } = null!;
    public EPropertyStatus Status { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Property() { }
    
    public static Property Create(HomeownerId ownerId, Address address, Geolocation geolocation, Region region, District district)
    {
        if (ownerId is null || string.IsNullOrWhiteSpace(ownerId.Value))
            throw new ArgumentException("OwnerId must be valid.");
        if (address is null)    throw new ArgumentNullException(nameof(address));
        if (geolocation is null) throw new ArgumentNullException(nameof(geolocation));

        var property = new Property
        {
            Id = PropertyId.NewPropertyId(),
            OwnerId = ownerId,
            Address = address,
            Geolocation = geolocation,
            Region = region,
            District = district,
            Status = EPropertyStatus.Created,
            IsActive = true,
        };

        property.RaiseDomainEvent(new PropertyCreatedEvent(property.Id, property.OwnerId, property.Address, property.Geolocation, DateTime.UtcNow));

        return property;
    }

    internal void UpdateAddress(Address newAddress)
    {
        if (Address.Equals(newAddress)) return;
        Address = newAddress;
        RaiseDomainEvent(new PropertyAddressUpdatedEvent(Id, newAddress, DateTime.UtcNow));
    }
    
    internal void UpdateGeolocation(Geolocation newGeolocation)
    {
        var previous = Geolocation;
        Geolocation  = newGeolocation;
        RaiseDomainEvent(new PropertyGeolocationUpdatedEvent(
            Id, OwnerId, previous, newGeolocation, DateTime.UtcNow));
    }
    internal void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        RaiseDomainEvent(new PropertyActivatedEvent(Id, DateTime.UtcNow));
    }

    internal void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
        RaiseDomainEvent(new PropertyDeactivatedEvent(Id, DateTime.UtcNow));
    }
    
    internal void Archive(string reason)
    {
        if (Status == EPropertyStatus.Archived) return;
        Status   = EPropertyStatus.Archived;
        IsActive = false;
        RaiseDomainEvent(new PropertyArchivedEvent(Id, OwnerId, reason, DateTime.UtcNow));
    }
    
    internal void RecordMaintenance(ServiceId serviceId, string technicianId, string workSummary, DateTime completedAt)
    {
        RaiseDomainEvent(new PropertyMaintenanceRecordedEvent(
            Id, serviceId, technicianId, workSummary, completedAt, DateTime.UtcNow));
    }

}