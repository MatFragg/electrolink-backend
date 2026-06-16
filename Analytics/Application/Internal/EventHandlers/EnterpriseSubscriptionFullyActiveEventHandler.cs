using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public class EnterpriseSubscriptionFullyActiveEventHandler(
    IConsumptionDashboardCommandService dashboardCommandService,
    ILogger<EnterpriseSubscriptionFullyActiveEventHandler> logger)
    : INotificationHandler<EnterpriseSubscriptionFullyActiveIntegrationEvent>
{
    public async Task Handle(EnterpriseSubscriptionFullyActiveIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Analytics BC] EnterpriseSubscriptionFullyActive: homeowner={HomeownerId}, subscription={SubscriptionId}",
            notification.HomeownerId, notification.SubscriptionId);

        await dashboardCommandService.InitializeDashboardAsync(
            notification.HomeownerId,
            notification.PropertyId,
            notification.DeviceIds,
            "EnterpriseBasic");
    }
}
