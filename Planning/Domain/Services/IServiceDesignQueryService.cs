using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

/// <summary>
/// Define los contratos para consultas de diseño de servicios.
/// Punto central para recuperar catálogos, recetas, elegibilidad, y información de matching.
/// </summary>
public interface IServiceDesignQueryService
{
    /// <summary>
    /// Obtiene los técnicos disponibles que cumplen con los criterios de matching.
    /// </summary>
    Task<IEnumerable<TechnicianId>> GetAvailableTechnicians(MatchingCriteria criteria);

    /// <summary>
    /// Calcula si un request es elegible para asignación automática.
    /// </summary>
    Task<bool> CalculateRequestEligibility(string requestId, HomeownerId homeownerId);

    /// <summary>
    /// Obtiene los requests en la cola de matching pendientes de asignación.
    /// </summary>
    Task<IEnumerable<string>> GetMatchingQueue(int limit, int offset);

    /// <summary>
    /// Obtiene el catálogo de servicios de un técnico.
    /// </summary>
    Task<ServiceCatalog?> GetServiceCatalogAsync(string catalogId);

    /// <summary>
    /// Valida si un request puede progresar según su estado actual.
    /// </summary>
    Task<bool> CanProgressRequestAsync(string requestId, string expectedStatus);
}

