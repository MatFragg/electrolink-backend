using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.QueryServices;

/// <summary>
/// Implementación de IServiceDesignQueryService.
/// Centraliza las consultas relacionadas con diseño de servicios, elegibilidad y matching.
/// </summary>
public class ServiceDesignQueryService(IServiceCatalogRepository catalogRepository, IServiceRequestRepository requestRepository,
IServiceAssignmentRepository assignmentRepository, ExternalProfilesService externalProfileService, ExternalSubscriptionsService externalSubscriptionsService, ExternalAssetsService externalAssetsService, ILogger<ServiceDesignQueryService> _logger) : IServiceDesignQueryService
{
    public async Task<ServiceCatalog?> Handle(GetServiceCatalogQuery query) 
        => await catalogRepository.FindByTechnicianIdAsync(query.TechnicianId);
    
    public async Task<ServiceRecipe?> Handle(GetServiceRecipeDetailsQuery query)
    {
        var catalog = await catalogRepository.FindByTechnicianIdAsync(
            query.TechnicianId);

        if (catalog is null) return null;

        var recipe = catalog.Recipes.FirstOrDefault(r =>
            r.Id == query.RecipeId);

        return recipe;
    }
    
    public async Task<RequestEligibility> Handle(GetRequestEligibilityQuery query)
    {
        var isActive = await externalProfileService.IsHomeownerActiveAsync(query.HomeownerId.Value);
        var hasProperties = await externalProfileService.HasPropertiesAsync(query.HomeownerId.Value);

        if (!isActive || !hasProperties)
            return new RequestEligibility(false, null, null, false, "PROFILE_INCOMPLETE");

        var eligibility = await externalSubscriptionsService.GetRemainingRequestsAsync(query.HomeownerId.Value);

        return new RequestEligibility(
            eligibility.canCreate,
            eligibility.planType,
            eligibility.remainingRequests,
            eligibility.canMarkAsPriority,
            eligibility.canCreate ? null : "MONTHLY_LIMIT_REACHED");
    }
    
    // OLD VERSION
    /*public async Task<IEnumerable<AvailableService>> Handle(GetAvailableServicesQuery query)
    {
        var request = await requestRepository.FindByIdAsync(query.RequestId);
        if (request is null || request.Geolocation is null) return [];

        var technicians = await externalProfileService.GetTechniciansInAreaAsync(
            request.Geolocation.Latitude, request.Geolocation.Longitude);

        var result = new List<AvailableService>();

        foreach (var tech in technicians)
        {
            var catalog = await catalogRepository.FindByTechnicianIdAsync(
                TechnicianId.From(tech.technicianId));

            if (catalog is null || catalog.Status != ECatalogStatus.Active) continue;

            foreach (var recipe in catalog.Recipes.Where(r => r.IsActive))
            {
                var stockCheck = await externalAssetsService.TechnicianHasStockForRecipeAsync(
                    tech.technicianId,
                    recipe.ComponentRequirements.Select(c => (c.ComponentTypeId, c.Quantity)).ToList());

                if (!stockCheck) continue;

                result.Add(new AvailableService(
                    recipe.Id.Value,
                    tech.technicianId,
                    tech.fullName,
                    tech.rating,
                    recipe.ServiceName,
                    recipe.ServiceDescription,
                    recipe.Pricing.TotalPrice.Amount,
                    recipe.EstimatedDuration.TotalMinutes,
                    stockCheck));
            }
        }

        return result;
    }
    */
    
    /*
     public async Task<IEnumerable<AvailableService>> Handle(GetAvailableServicesQuery query)
    {
        _logger.LogInformation("1. Iniciando búsqueda de servicios para RequestId: {RequestId}", query.RequestId.Value);

        var request = await requestRepository.FindByIdAsync(query.RequestId);
        if (request is null)
        {
            _logger.LogWarning("-> Request {RequestId} no encontrado.", query.RequestId.Value);
            return [];
        }

        if (request.Geolocation is null)
        {
            _logger.LogWarning("-> Request {RequestId} no tiene geolocalización.", query.RequestId.Value);
            return [];
        }

        var technicians = (await externalProfileService.GetTechniciansInAreaAsync(
            request.Geolocation.Latitude, request.Geolocation.Longitude)).ToList();

        _logger.LogInformation("2. Se encontraron {Count} técnicos en el área.", technicians.Count);

        var result = new List<AvailableService>();

        foreach (var tech in technicians)
        {
            _logger.LogInformation("-> Evaluando técnico: {TechnicianId} ({Name})", tech.technicianId, tech.fullName);

            var catalog = await catalogRepository.FindByTechnicianIdAsync(TechnicianId.From(tech.technicianId));

            if (catalog is null)
            {
                _logger.LogInformation("   - Descartado: No tiene catálogo creado.");
                continue;
            }

            if (catalog.Status != ECatalogStatus.Active)
            {
                _logger.LogInformation("   - Descartado: Su catálogo está en estado {Status}.", catalog.Status);
                continue;
            }

            var activeRecipes = catalog.Recipes.Where(r => r.IsActive).ToList();
            _logger.LogInformation("   - El catálogo tiene {Count} recetas activas.", activeRecipes.Count);

            foreach (var recipe in activeRecipes)
            {
                var componentRequirements = recipe.ComponentRequirements.Select(c => (c.ComponentTypeId, c.Quantity)).ToList();
                
                var stockCheck = await externalAssetsService.TechnicianHasStockForRecipeAsync(
                    tech.technicianId, componentRequirements);

                if (!stockCheck)
                {
                    _logger.LogInformation("      x Receta '{ServiceName}' descartada: El técnico NO tiene el stock exacto en su inventario.", recipe.ServiceName);
                    continue;
                }

                _logger.LogInformation("✓ Receta '{ServiceName}' ACEPTADA. Hay stock.", recipe.ServiceName);

                result.Add(new AvailableService(
                    recipe.Id.Value,
                    tech.technicianId,
                    tech.fullName,
                    tech.rating,
                    recipe.ServiceName,
                    recipe.ServiceDescription,
                    recipe.Pricing.TotalPrice.Amount,
                    recipe.EstimatedDuration.TotalMinutes,
                    stockCheck));
            }
        }

        _logger.LogInformation("3. Búsqueda completada para Request {RequestId}. Total servicios disponibles: {Count}.", query.RequestId.Value, result.Count);

        return result;
    }
    */
    
    public async Task<IEnumerable<AvailableService>> Handle(GetAvailableServicesQuery query) {
        var request = await requestRepository.FindByIdAsync(query.RequestId);
        if (request is null || request.Geolocation is null) return [];

        var technicians = (await externalProfileService.GetTechniciansInAreaAsync(
            request.Geolocation.Latitude, request.Geolocation.Longitude)).ToList();

        // categoria -> lista de (precio, duracion) por tecnico que puede hacerla
        var categoryData = new Dictionary<EServiceCategory, List<(decimal price, int duration)>>();

        foreach (var tech in technicians)
        {
            var catalog = await catalogRepository.FindByTechnicianIdAsync(
                TechnicianId.From(tech.technicianId));

            if (catalog is null || catalog.Status != ECatalogStatus.Active) continue;

            foreach (var recipe in catalog.Recipes.Where(r => r.IsActive))
            {
                var requirements = recipe.ComponentRequirements
                    .Select(c => (c.ComponentTypeId, c.Quantity))
                    .ToList();

                var hasStock = await externalAssetsService.TechnicianHasStockForRecipeAsync(
                    tech.technicianId, requirements);

                if (!hasStock) continue;

                if (!categoryData.ContainsKey(recipe.ServiceCategory))
                    categoryData[recipe.ServiceCategory] = new List<(decimal, int)>();

                categoryData[recipe.ServiceCategory].Add(
                    (recipe.Pricing.TotalPrice.Amount, recipe.EstimatedDuration.TotalMinutes));
            }
        }

        return categoryData.Select(kvp => new AvailableService(
            ServiceCategory: kvp.Key.ToString(),
            CategoryDisplayName: GetCategoryDisplayName(kvp.Key),
            MinPrice: kvp.Value.Min(x => x.price),
            MaxPrice: kvp.Value.Max(x => x.price),
            EstimatedDurationMinutes: (int)kvp.Value.Average(x => x.duration),
            TechniciansAvailable: kvp.Value.Count));
    }

private static string GetCategoryDisplayName(EServiceCategory category) => category switch
{
    EServiceCategory.SolarInstallation    => "Instalación Solar",
    EServiceCategory.ElectricalMaintenance => "Mantenimiento Eléctrico",
    EServiceCategory.Repair               => "Reparación",
    EServiceCategory.Inspection           => "Inspección",
    EServiceCategory.Upgrade              => "Actualización",
    _                                     => category.ToString()
};


    public async Task<ServiceRequest?> Handle(GetServiceRequestSummaryQuery query)
    {
        var request = await requestRepository.FindByIdAsync(query.RequestId);

        if (request is null || request.HomeownerId != query.HomeownerId)
            return null;

        return request;
    }

    public async Task<MatchingQueue> Handle(GetMatchingQueueQuery query)
    {
        var pending = await requestRepository.FindPendingAssignmentAsync();

        var sorted = pending.OrderByDescending(r => r.IsPriority)
            .ThenBy(r => r.CreatedDate)
            .ToList();

        var priorityQueue = sorted.Where(r => r.IsPriority)
            .Select(r => new QueuedRequest(r.RequestId.Value, r.HomeownerId.Value, r.IsPriority, r.CreatedDate.GetValueOrDefault()))
            .ToList();

        var normalQueue = sorted.Where(r => !r.IsPriority)
            .Select(r => new QueuedRequest(r.RequestId.Value, r.HomeownerId.Value, r.IsPriority, r.CreatedDate.GetValueOrDefault()))
            .ToList();

        return new MatchingQueue(priorityQueue, normalQueue, sorted.Count);
    }
}


