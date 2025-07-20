using Hampcoders.Electrolink.API.Assets.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Assets.Domain.Model.Queries;

namespace Hampcoders.Electrolink.API.Assets.Domain.Services;

public interface IPropertyQueryService
{
    Task<Property?> Handle(GetPropertyByIdQuery query);

    /// <summary>
    /// Maneja la consulta para obtener una lista filtrada de propiedades de un propietario.
    /// </summary>
    Task<IEnumerable<Property>> Handle(GetAllPropertiesByOwnerIdQuery query);
    
}