using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class Property
{
    private void UpdateAddress(Address newAddress)
    {
        if (newAddress == null) throw new ArgumentNullException(nameof(newAddress));

        if (Address.Equals(newAddress)) return; 

        Address = newAddress;
        _domainEvents.Add(new PropertyAddressUpdatedEvent(
            Id,
            newAddress.Street, 
            newAddress.Latitude, 
            newAddress.Longitude, 
            DateTime.UtcNow
        ));
    }
    private void Deactivate()
    {
        if (!IsActive) return; 

        IsActive = false;
        _domainEvents.Add(new PropertyDeactivatedEvent(Id, DateTime.UtcNow));
    }
    private void Activate()
    {
        if (IsActive) return; 

        IsActive = true;
        _domainEvents.Add(new PropertyActivatedEvent(Id, DateTime.UtcNow));
    }
    
    
}