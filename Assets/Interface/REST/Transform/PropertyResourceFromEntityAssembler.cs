using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Interface.REST.Resources;

namespace Hampcoders.Electrolink.API.Assets.Interface.REST.Transform;

public static class PropertyResourceFromEntityAssembler
{
    // Convierte el Agregado del Dominio en un Resource para la respuesta de la API
    public static PropertyResource ToResourceFromEntity(Property entity)
    {
        var fullAddress = $"{entity.Address.Street} {entity.Address.Number}, {entity.District.Name}, {entity.Address.City}";

        return new PropertyResource(
            entity.Id.Id,
            fullAddress,
            entity.Region.Name,
            entity.District.Name,
            entity.Address.Latitude,
            entity.Address.Longitude
            //entity.Status.ToString()
            );
    }
}