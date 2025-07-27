using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands.Properties;
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
    public bool IsActive { get; private set; } = true;
    
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();
    public Property()
    {

    }
    
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
    }

    public Property(CreatePropertyCommand command) : this(command.OwnerId,command.Address,command.Region,command.District)
    {
    }
    
     public void Handle(UpdatePropertyAddressCommand command)
    {
        
        if (command.Id != Id.Id) 
            throw new InvalidOperationException($"Command ID {command.Id} does not match Property ID {Id.Id}.");

        UpdateAddress(command.NewAddress); 
    }

    public void Handle(UpdatePropertyCommand command)
    {
        if (command.Id != Id.Id) return;

        // Validaciones de negocio (ej. si la propiedad debe estar activa para actualizar)
        // if (!IsActive) throw new InvalidOperationException("No se puede actualizar una propiedad inactiva.");
        if (command.OwnerId == Guid.Empty) throw new ArgumentException("New Owner ID must be valid.", nameof(command.OwnerId));
        if (command.Address == null) throw new ArgumentNullException(nameof(command.Address));
        if (command.RegionName == null) throw new ArgumentNullException(nameof(command.RegionName));
        if (command.DistrictName == null) throw new ArgumentNullException(nameof(command.DistrictName));

        OwnerId = new OwnerId(command.OwnerId);

        Address = command.Address;

        Region = new Region(command.RegionName);
        District = new District(command.DistrictName);

        _domainEvents.Add(new PropertyDetailsUpdatedEvent(Id.Id, OwnerId, Address, Region, District, DateTime.UtcNow)); 
    }
    
    public void Handle(DeactivatePropertyCommand command)
    {
        if (command.PropertyId != Id.Id) return; 

        Deactivate(); 
    }

    public void Handle(ActivatePropertyCommand command)
    {
        if (command.PropertyId != Id.Id) return; 

        Activate(); 
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}