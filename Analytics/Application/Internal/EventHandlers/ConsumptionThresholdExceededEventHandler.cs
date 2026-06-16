using Hampcoders.Electrolink.API.Analytics.Domain.Model.Events;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public class ConsumptionThresholdExceededEventHandler(
    IAlertLogCommandService alertLogCommandService)
    : INotificationHandler<ConsumptionThresholdExceeded>
{
    public async Task Handle(ConsumptionThresholdExceeded notification, CancellationToken cancellationToken)
    {
        await alertLogCommandService.RecordThresholdAlertAsync(
            notification.HomeownerId,
            notification.CircuitId,
            notification.ConsumedKilowattHours,
            notification.DashboardId);
    }
}
