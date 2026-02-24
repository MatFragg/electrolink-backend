using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using PropertyId = Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects.PropertyId;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public class Property : BaseAggregateRoot
{
    public PropertyId Id { get; private set; } = null!;
    public HomeownerId OwnerId { get; private set; }
    public Address Address { get; private set; }
    public Region Region { get; private set; }
    public District District { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Property()
    {

    }
    
    public Property(HomeownerId ownerId, Address address, Region region, District district) : this()
    {
        if (ownerId == null || ownerId.Value == string.Empty) throw new ArgumentException("Owner ID must be valid.", nameof(ownerId));
        if (address == null) throw new ArgumentNullException(nameof(address));
        if (region == null) throw new ArgumentNullException(nameof(region));
        if (district == null) throw new ArgumentNullException(nameof(district));
        
        OwnerId = ownerId;
        Address = address;
        Region = region;
        District = district;
    }

    public Property(CreatePropertyCommand command) : this(command.HomeownerId,command.Address,command.Region,command.District)
    {
    }
    
     public void Handle(UpdatePropertyAddressCommand command)
    {
        
        if (PropertyId.From(command.PropertyId) != Id) 
            throw new InvalidOperationException($"Command ID {command.PropertyId} does not match Property ID {Id.Value}.");

        UpdateAddress(command.NewAddress); 
    }

    public void Handle(UpdatePropertyCommand command)
    {
        if (PropertyId.From(command.PropertyId) != Id) return;

        // Validaciones de negocio (ej. si la propiedad debe estar activa para actualizar)
        // if (!IsActive) throw new InvalidOperationException("No se puede actualizar una propiedad inactiva.");
        if (command.HomeownerId == string.Empty) throw new ArgumentException("New Owner ID must be valid.", nameof(command.HomeownerId));
        if (command.Address == null) throw new ArgumentNullException(nameof(command.Address));
        if (command.RegionName == null) throw new ArgumentNullException(nameof(command.RegionName));
        if (command.DistrictName == null) throw new ArgumentNullException(nameof(command.DistrictName));

        OwnerId = HomeownerId.From(command.HomeownerId);

        Address = command.Address;

        Region = new Region(command.RegionName);
        District = new District(command.DistrictName);
    }
    
    public void Handle(DeactivatePropertyCommand command)
    {
        if (PropertyId.From(command.PropertyId) != Id) return; 

        Deactivate(); 
    }

    public void Handle(ActivatePropertyCommand command)
    {
        if (PropertyId.From(command.PropertyId) != Id) return; 

        Activate(); 
    }
    
    private void UpdateAddress(Address newAddress)
    {
        if (newAddress == null) throw new ArgumentNullException(nameof(newAddress));

        if (Address.Equals(newAddress)) return; 

        Address = newAddress;
    }
    private void Deactivate()
    {
        if (!IsActive) return; 

        IsActive = false;
    }
    private void Activate()
    {
        if (IsActive) return; 

        IsActive = true;
    }
}