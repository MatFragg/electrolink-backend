using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Assets.Domain.ModeL.Commands.Properties;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.CommandServices;

public class PropertyCommandService(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork) : IPropertyCommandService
{
    public async Task<Property?> Handle(CreatePropertyCommand command)
    {
        var property = new Property(command);
        await propertyRepository.AddAsync(property);
        await unitOfWork.CompleteAsync();
        return property;
    }

    /*public async Task<Property?> Handle(AddPhotoToPropertyCommand command)
    {
        var property = await propertyRepository.FindByIdAsync(new PropertyId(command.Id));
        if (property is null) throw new ArgumentException("Property not found.");

        // Delegamos la lógica al manejador del agregado
        property.Handle(command);

        // EF Core Change Tracking se encarga de detectar la actualización
        await unitOfWork.CompleteAsync();
        return property;
    }*/

    public async Task<Property?> Handle(UpdatePropertyAddressCommand command)
    {
        var property = await propertyRepository.FindByIdAsync(new PropertyId(command.Id));
        if (property is null) throw new ArgumentException("Property not found.");

        property.Handle(command);
        await unitOfWork.CompleteAsync();
        return property;
    }

    /*public async Task<Property?> Handle(DeactivatePropertyCommand command)
    {
        // Para que sea seguro, el comando también debería tener el OwnerId
        // Por ahora, usamos el método inseguro, pero lo ideal es validar el dueño
        var property = await propertyRepository.FindByIdAsync(new PropertyId(command.Id));
            
        if (property == null) return null;

        // Le decimos al Agregado que ejecute su lógica interna
        property.Handle(command);
            
        // Guardamos los cambios a través de la Unit of Work
        await unitOfWork.CompleteAsync(); // Usamos unitOfWork, no el Context directamente
            
        return property;
    }*/
    
    public async Task<Property?> Handle(UpdatePropertyCommand command)
    {
        var property = await propertyRepository.FindByIdAndOwnerIdAsync(
            new PropertyId(command.Id), 
            new OwnerId(command.OwnerId)
        );
        
        if (property is null) return null; 

        property.Handle(command);

        await unitOfWork.CompleteAsync();
        
        return property;
    }
    
    public async Task<bool> Handle(DeletePropertyCommand command)
    {
        var property = await propertyRepository.FindByIdAsync(new PropertyId(command.Id));
        if (property is null) return false;

        propertyRepository.Remove(property);
        await unitOfWork.CompleteAsync();
        return true;
    }
    
    
}