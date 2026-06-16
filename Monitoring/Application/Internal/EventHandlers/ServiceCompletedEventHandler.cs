using Hampcoders.Electrolink.API.Monitoring.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.EventHandlers;

public class ServiceCompletedEventHandler(
    IExternalAnalyticsService analyticsService,
    ILogger<ServiceCompletedEventHandler> logger)
    : INotificationHandler<ServiceCompletedEvent>
{
    public async Task Handle(ServiceCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Monitoring BC] ServiceExecution {ExecutionId} completed at {CompletedAt}.",
            notification.ExecutionId.Value, notification.CompletedAt);

        await analyticsService.NotifyCompletionAsync(
            notification.ExecutionId.Value,
            notification.TechnicianId.Value,
            notification.CompletedAt);
    }
}
