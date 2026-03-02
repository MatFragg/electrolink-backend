using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interfaces.REST.Transform;

public static class PropertyResourceFromEntityAssembler
{
    // Convierte el Agregado del Dominio en un Resource para la respuesta de la API
    public static PropertyResource ToResourceFromEntity(Property entity) 
        => new PropertyResource(
            entity.Id.Value,
            entity.OwnerId.Value, 
            new AddressResource(
                entity.Address.Street, entity.Address.District, entity.Address.City,
                entity.Address.Country, entity.Address.PostalCode),
            new GeolocationResource(
                entity.Geolocation.Latitude, entity.Geolocation.Longitude,
                entity.Geolocation.Accuracy, entity.Geolocation.Source),
            entity.Status.ToString(),
            entity.IsActive
        );
}