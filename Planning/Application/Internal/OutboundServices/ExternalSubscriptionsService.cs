namespace Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;

/// <summary>
/// DTO para información del plan de suscripción
/// </summary>
public record SubscriptionPlanDto(
    string PlanName,
    bool IsPremium,
    int? MonthlyRequestLimit,
    bool CanMarkAsPriority
);

/// <summary>
/// Anti-Corruption Layer para comunicación con Subscriptions Bounded Context
/// </summary>
public class ExternalSubscriptionsService(ILogger<ExternalSubscriptionsService> logger)
{
    /// <summary>
    /// Obtiene el plan actual de un homeowner
    /// </summary>
    public async Task<SubscriptionPlanDto> GetCurrentPlanAsync(Guid homeownerId)
    {
        logger.LogInformation($"[Planning BC] ACL: Getting current plan for homeowner {homeownerId} from Subscriptions BC");
        
        // TODO: Implementar llamada al Subscriptions BC
        // var subscription = await _subscriptionsContextFacade.GetActiveSubscriptionAsync(homeownerId);
        
        await Task.Delay(10);
        
        // Placeholder - retornar plan básico por defecto
        return new SubscriptionPlanDto(
            PlanName: "Basic",
            IsPremium: false,
            MonthlyRequestLimit: 3,
            CanMarkAsPriority: false
        );
    }

    /// <summary>
    /// Verifica si un técnico tiene plan Premium (puede tener catálogo)
    /// </summary>
    public async Task<bool> IsTechnicianPremiumAsync(Guid technicianId)
    {
        logger.LogInformation($"[Planning BC] ACL: Checking if technician {technicianId} is premium");
        
        // TODO: Implementar verificación con Subscriptions BC
        // var subscription = await _subscriptionsContextFacade.GetActiveSubscriptionAsync(technicianId);
        // return subscription?.IsPremium ?? false;
        
        await Task.Delay(10);
        
        // Por ahora retornamos true para permitir desarrollo
        return true;
    }

    /// <summary>
    /// Verifica si un homeowner puede crear una nueva solicitud (límite mensual)
    /// </summary>
    public async Task<(bool CanCreate, string? Reason)> CanCreateRequestAsync(Guid homeownerId)
    {
        logger.LogInformation($"[Planning BC] ACL: Checking if homeowner {homeownerId} can create request");
        
        var plan = await GetCurrentPlanAsync(homeownerId);
        
        // TODO: Obtener conteo de requests del mes actual
        // var currentMonthRequests = await _requestRepository.CountRequestsThisMonthAsync(homeownerId);
        var currentMonthRequests = 0; // Placeholder
        
        if (plan.MonthlyRequestLimit.HasValue && currentMonthRequests >= plan.MonthlyRequestLimit.Value)
        {
            return (false, $"Monthly request limit reached ({plan.MonthlyRequestLimit} requests). Upgrade to Premium for unlimited requests.");
        }
        
        return (true, null);
    }

    /// <summary>
    /// Verifica si un homeowner puede marcar una solicitud como prioritaria
    /// </summary>
    public async Task<bool> CanMarkAsPriorityAsync(Guid homeownerId)
    {
        logger.LogInformation($"[Planning BC] ACL: Checking if homeowner {homeownerId} can mark as priority");
        
        var plan = await GetCurrentPlanAsync(homeownerId);
        return plan.CanMarkAsPriority;
    }
}

