using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class SubscriptionEligibilityAclAssembler
{
    /// <summary>
    /// Convierte un agregado de Subscription y su Plan asociado en un SubscriptionEligibilityResource.
    /// </summary>
    /// <param name="subscription">El agregado de Subscription.</param>
    /// <param name="plan">El agregado de Plan asociado.</param>
    /// <returns>Un SubscriptionEligibilityResource.</returns>
    public static SubscriptionEligibilityResource ToResourceFromEntities(
        Subscription subscription,
        Plan plan)
    {
        // La condición para 'IsPremium' debe basarse en PremiumAccess
        // y no en el MonetizationType, ya que este último es solo la frecuencia de pago.
        bool isPremium = subscription.PremiumAccess != null && subscription.PremiumAccess.IsActive(DateTime.UtcNow);

        // La capacidad de crear solicitudes prioritarias está ligada directamente a ser premium.
        bool canCreatePriorityRequest = isPremium;

        return new SubscriptionEligibilityResource(
            UserId: subscription.UserId.Value,
            PlanName: plan.Name,
            IsPremium: isPremium,
            UsageLimit: null, // Sigue siendo null, ya que no está directamente en los agregados actuales
            CurrentUsage: null, // Sigue siendo null
            CanCreatePriorityRequest: canCreatePriorityRequest
        );
    }

    /// <summary>
    /// Método para retornar un recurso de elegibilidad para un usuario sin suscripción activa,
    /// representando un plan básico o por defecto, sin detalles de suscripción.
    /// </summary>
    /// <param name="userId">El ID del usuario.</param>
    /// <returns>Un SubscriptionEligibilityResource para un plan básico.</returns>
    public static SubscriptionEligibilityResource ToBasicResource(int userId)
    {
        return new SubscriptionEligibilityResource(
            UserId: userId,
            PlanName: "Basic",
            IsPremium: false,
            UsageLimit: 0,
            CurrentUsage: 0,
            CanCreatePriorityRequest: false
        );
    }
}