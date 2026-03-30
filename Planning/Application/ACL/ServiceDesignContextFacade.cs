using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Interfaces.ACL;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.ACL;

/// <summary>
/// Implementación del Anti-Corruption Layer (ACL).
/// Traduce agregados del dominio a DTOs para consumo de otros BCs.
/// Protege la integridad interna del BC de cambios externos.
/// </summary>
public class ServiceDesignContextFacade : IServiceDesignContextFacade
{
    private readonly IServiceCatalogRepository _catalogRepository;
    private readonly IServiceRequestRepository _requestRepository;
    private readonly ILogger<ServiceDesignContextFacade> _logger;

    public ServiceDesignContextFacade(
        IServiceCatalogRepository catalogRepository,
        IServiceRequestRepository requestRepository,
        ILogger<ServiceDesignContextFacade> logger)
    {
        _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceCatalogDto?> GetServiceCatalogAsync(string catalogId)
    {
        try
        {
            var catalog = await _catalogRepository.FindByIdAsync(CatalogId.From(catalogId));
            
            if (catalog == null)
                return null;

            return new ServiceCatalogDto(
                catalog.Id.Value,
                catalog.TechnicianId.Value,
                catalog.Status.ToString(),
                catalog.Recipes.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Planning ACL] Error getting service catalog");
            throw;
        }
    }

    public async Task<ServiceRecipeDetailDto?> GetServiceRecipeAsync(string recipeId)
    {
        try
        {
            // Buscar en todos los catálogos (en una implementación real, habría índices)
            var allCatalogs = await _catalogRepository.FindAllAsync();
            var recipe = allCatalogs
                .SelectMany(c => c.Recipes)
                .FirstOrDefault(r => r.Id.Value == recipeId);

            if (recipe == null)
                return null;

            var (hours, minutes) = recipe.EstimatedDuration.ToHoursAndMinutes();
            return new ServiceRecipeDetailDto(
                recipe.Id.Value,
                recipe.ServiceName.Value,
                recipe.ServiceCategory.ToString(),
                recipe.Pricing.TotalPrice.Amount,
                recipe.Pricing.TotalPrice.Currency,
                hours,
                minutes,
                recipe.IsActive,
                recipe.TimesRequested);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Planning ACL] Error getting service recipe");
            throw;
        }
    }

    public async Task<IEnumerable<AvailableServiceDto>> GetAvailableServicesAsync(
        double latitude,
        double longitude,
        IReadOnlyList<string> componentTypeIds)
    {
        try
        {
            var allCatalogs = await _catalogRepository.FindAllAsync();
            
            var availableServices = allCatalogs
                .SelectMany(c => c.Recipes.Where(r => r.IsActive))
                .Where(r => HasRequiredComponents(r, componentTypeIds))
                .Select(r => 
                {
                    var (hours, minutes) = r.EstimatedDuration.ToHoursAndMinutes();
                    return new AvailableServiceDto(
                        r.Id.Value,
                        r.ServiceName.Value,
                        r.Pricing.TotalPrice.Amount,
                        r.Pricing.TotalPrice.Currency,
                        hours);
                })
                .ToList();

            return availableServices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Planning ACL] Error getting available services");
            throw;
        }
    }

    public async Task<bool> IsRequestEligibleForMatchingAsync(string requestId, string homeownerId)
    {
        try
        {
            var request = await _requestRepository.FindByIdAsync(RequestId.From(requestId));
            
            if (request == null)
                return false;

            return request.Status == RequestStatus.PendingAssignment &&
                   request.SelectedRecipeId != null &&
                   request.PropertySnapshot != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Planning ACL] Error checking request eligibility");
            throw;
        }
    }

    public async Task<ServiceRequestDto?> GetServiceRequestAsync(string requestId)
    {
        try
        {
            var request = await _requestRepository.FindByIdAsync(RequestId.From(requestId));
            
            if (request == null)
                return null;

            return new ServiceRequestDto(
                request.Id.Value,
                request.HomeownerId.Value,
                request.Status.ToString(),
                request.IsPriority);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Planning ACL] Error getting service request");
            throw;
        }
    }

    private bool HasRequiredComponents(ServiceRecipe recipe, IReadOnlyList<string> requiredComponentTypes)
    {
        if (!requiredComponentTypes.Any())
            return true;

        var recipeComponentIds = recipe.ComponentRequirements
            .Select(c => c.ComponentTypeId)
            .ToHashSet();

        return requiredComponentTypes.All(required => recipeComponentIds.Contains(required));
    }
}



