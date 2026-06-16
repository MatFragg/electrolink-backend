using Hampcoders.Electrolink.API.Monitoring.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.EventHandlers;

public class ServiceExecutionCancelledEventHandler(
    IExternalAnalyticsService analyticsService,
    ILogger<ServiceExecutionCancelledEventHandler> logger)
    : INotificationHandler<ServiceExecutionCancelledEvent>
{
    public async Task Handle(ServiceExecutionCancelledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Monitoring BC] ServiceExecution {ExecutionId} cancelled. Reason: {Reason}.",
            notification.ExecutionId.Value, notification.Reason);

        await analyticsService.NotifyCancellationAsync(
            notification.ExecutionId.Value,
            notification.Reason,
            notification.CancelledAt);
    }
}
