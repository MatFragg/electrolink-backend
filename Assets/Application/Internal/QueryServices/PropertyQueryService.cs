using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Assets.Domain.Repositories;
using Hampcoders.Electrolink.API.Assets.Domain.Services;

namespace Hampcoders.Electrolink.API.Assets.Application.Internal.QueryServices;

public class PropertyQueryService(IPropertyRepository propertyRepository) : IPropertyQueryService
{
    /// <summary>
    /// Maneja la consulta para obtener una propiedad específica por su ID,
    /// asegurando que pertenezca al propietario correcto.
    /// </summary>
    public async Task<Property?> Handle(GetPropertyByIdQuery query)
    {
        // Esta línea ahora funcionará porque el 'using' le dice al compilador
        // dónde encontrar 'PropertyId' y 'OwnerId'.
        return await propertyRepository.FindByIdAndOwnerIdAsync(
            new PropertyId(query.PropertyId),
            new OwnerId(query.OwnerId)
        );
    }

    /// <summary>
    /// Maneja la consulta para obtener todas las propiedades de un propietario,
    /// aplicando los filtros opcionales de búsqueda.
    /// </summary>
    public async Task<IEnumerable<Property>> Handle(GetAllPropertiesByOwnerIdQuery query)
    {
        // Llama al método de búsqueda flexible y unificado del repositorio.
        return await propertyRepository.GetAllFilteredAsync(
            new OwnerId(query.OwnerId),
            query.City,
            query.District,
            query.Region,
            query.Street
        );
    }
}