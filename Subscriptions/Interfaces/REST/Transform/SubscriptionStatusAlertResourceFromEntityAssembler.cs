using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;

public static class SubscriptionStatusAlertResourceFromEntityAssembler
{
    public static SubscriptionStatusAlertResource ToResource(Subscription subscription, string? portalUrl)
        => new(
            Status: subscription.Status.ToString(),
            PlanType: subscription.PlanType.ToString(),
            GracePeriodEndsAt: subscription.GracePeriodEndsAt?.ToString("O") ?? "N/A",
            Message: $"Tu pago falló. Actualiza tu método de pago antes del " +
                                $"{subscription.GracePeriodEndsAt.Value:dd 'de' MMMM} para mantener tu plan Premium.",
            CustomerPortalUrl: portalUrl);
}
