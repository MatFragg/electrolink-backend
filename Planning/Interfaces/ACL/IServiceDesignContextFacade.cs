using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.ACL;

/// <summary>
/// Interfaz de Anti-Corruption Layer (ACL).
/// Define las capacidades del Planning BC que otros Bounded Contexts pueden usar.
/// Aislamiento de cambios internos del dominio.
/// </summary>
public interface IServiceDesignContextFacade
{
    /// <summary>
    /// Obtiene el catálogo de servicios de un técnico.
    /// </summary>
    Task<ServiceCatalogDto?> GetServiceCatalogAsync(string catalogId);

    /// <summary>
    /// Obtiene los detalles de una receta de servicio.
    /// </summary>
    Task<ServiceRecipeDetailDto?> GetServiceRecipeAsync(string recipeId);

    /// <summary>
    /// Obtiene servicios disponibles según criterios de búsqueda.
    /// </summary>
    Task<IEnumerable<AvailableServiceDto>> GetAvailableServicesAsync(
        double latitude,
        double longitude,
        IReadOnlyList<string> componentTypeIds);

    /// <summary>
    /// Valida si un request es elegible para asignación automática.
    /// </summary>
    Task<bool> IsRequestEligibleForMatchingAsync(string requestId, string homeownerId);

    /// <summary>
    /// Obtiene el resumen de un request.
    /// </summary>
    Task<ServiceRequestDto?> GetServiceRequestAsync(string requestId);
}

// DTOs para el ACL
public record ServiceCatalogDto(
    string CatalogId,
    string TechnicianId,
    string Status,
    int RecipeCount);

public record ServiceRecipeDetailDto(
    string RecipeId,
    string ServiceName,
    string ServiceCategory,
    decimal TotalPrice,
    string Currency,
    int EstimatedHours,
    int EstimatedMinutes,
    bool IsActive,
    int TimesRequested);

public record AvailableServiceDto(
    string RecipeId,
    string ServiceName,
    decimal TotalPrice,
    string Currency,
    int EstimatedHours);

public record ServiceRequestDto(
    string RequestId,
    string HomeownerId,
    string Status,
    bool IsPriority);

