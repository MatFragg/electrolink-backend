using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public class ConsumptionThresholdsUpdatedEventHandler(
    IConsumptionDashboardCommandService dashboardCommandService,
    ILogger<ConsumptionThresholdsUpdatedEventHandler> logger)
    : INotificationHandler<ConsumptionThresholdsUpdatedIntegrationEvent>
{
    public async Task Handle(ConsumptionThresholdsUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Analytics BC] ConsumptionThresholdsUpdated: homeowner={HomeownerId}, low={Low}, high={High}",
            notification.HomeownerId, notification.LowThreshold, notification.HighThreshold);

        var thresholds = new Dictionary<string, decimal>
        {
            { "Low", notification.LowThreshold },
            { "High", notification.HighThreshold }
        };

        await dashboardCommandService.UpdateConsumptionThresholdsAsync(
            notification.HomeownerId, thresholds);
    }
}
