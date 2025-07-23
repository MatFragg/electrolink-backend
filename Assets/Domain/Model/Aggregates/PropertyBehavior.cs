using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Events.Properties;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class Property
{
    public Property()
    {

    }
    
    private void UpdateAddress(Address newAddress)
    {
        Address = newAddress;
    }
    
    public void Handle(UpdatePropertyAddressCommand command)
    {
        if (command.Id == Id.Id)
            UpdateAddress(command.NewAddress);
    }
    
    public void Handle(UpdatePropertyCommand command)
    {
        if (command.Id != Id.Id) return;

        // Convertir Guid a OwnerId
        OwnerId = new OwnerId(command.OwnerId);

        // Asignar la dirección
        Address = command.Address;

        // Crear objetos Region y District con los 2 parámetros requeridos
        Region = new Region(command.RegionName);
        District = new District(command.DistrictName);
    }
    
    public void DeactivateProperty()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException($"Property {Id.Id} is already deactivated.");
        }
        IsActive = false;
        _domainEvents.Add(new PropertyDeactivatedEvent(Id, DateTime.UtcNow));
    }

    public void ActivateProperty()
    {
        if (IsActive)
        {
            throw new InvalidOperationException($"Property {Id.Id} is already activated.");
        }
        IsActive = true;
        _domainEvents.Add(new PropertyActivatedEvent(Id, DateTime.UtcNow));
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}