using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;

public partial class Property
{
    //public EPropertyStatus Status { get; private set; }
    //public PropertyPhoto? Photo { get; private set; } // Puede ser nulo


    public Property()
    {

    }

    /// <summary>
    /// Establece o elimina la foto de la propiedad.
    /// </summary>
    /// <param name="photoUrl">La URL de la nueva foto, o null para eliminar la existente.</param>
    
    /*public void SetPhoto(string? photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            Photo = null;
            return;
        }
        Photo = new PropertyPhoto(photoUrl);
    }*/
    
    private void UpdateAddress(Address newAddress)
    {
        /*if (Status == EPropertyStatus.Inactive)
            throw new InvalidOperationException("Cannot update the address of an inactive property.");*/
        Address = newAddress;
    }
    
    /*private void Deactivate()
    {
        if (Status == EPropertyStatus.Inactive) return;
        Status = EPropertyStatus.Inactive;
    }*/

    /*public void Handle(AddPhotoToPropertyCommand command)
    {
        if(command.Id == Id.Id)
            SetPhoto(command.PhotoUrl);
    }*/
    
    public void Handle(UpdatePropertyAddressCommand command)
    {
        if (command.Id == Id.Id)
            UpdateAddress(command.NewAddress);
    }
    
    /*public void Handle(DeactivatePropertyCommand command)
    {
        if (command.Id == Id.Id)
            Deactivate();
    }*/
    
    public void Handle(UpdatePropertyCommand command)
    {
        if (command.Id != Id.Id) return;

        // Verificar que la propiedad esté activa
        //if (Status == EPropertyStatus.Inactive)
        ////  throw new InvalidOperationException("No se puede actualizar una propiedad inactiva.");

        // Convertir Guid a OwnerId
        OwnerId = new OwnerId(command.OwnerId);

        // Asignar la dirección
        Address = command.Address;

        // Crear objetos Region y District con los 2 parámetros requeridos
        Region = new Region(command.RegionName);
        District = new District(command.DistrictName);
    }
}