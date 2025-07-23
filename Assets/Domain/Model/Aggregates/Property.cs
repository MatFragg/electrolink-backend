using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class Property
{
    public PropertyId Id { get; private set; } = PropertyId.NewId();
    public OwnerId OwnerId { get; private set; }
    public Address Address { get; private set; }
    public Region Region { get; private set; }
    public District District { get; private set; }
    public bool IsActive { get; private set; }
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();
    public Property(OwnerId ownerId, Address address, Region region, District district) : this()
    {
        if (ownerId == null || ownerId.Id == Guid.Empty) throw new ArgumentException("Owner ID must be valid.", nameof(ownerId));
        if (address == null) throw new ArgumentNullException(nameof(address));
        if (region == null) throw new ArgumentNullException(nameof(region));
        if (district == null) throw new ArgumentNullException(nameof(district));
        
        OwnerId = ownerId;
        Address = address;
        Region = region;
        District = district;
        IsActive = true;
        
        _domainEvents.Add(new PropertyCreatedEvent(Id, OwnerId, address.Street, DateTime.UtcNow));
    }

    public Property(CreatePropertyCommand command) : this(command.OwnerId,command.Address,command.Region,command.District)
    {
    }
    
}