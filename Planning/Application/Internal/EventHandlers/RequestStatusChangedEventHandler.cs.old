using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Microsoft.Extensions.Logging;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Handles RequestStatusChangedEvent for tracking and audit purposes.
/// </summary>
public class RequestStatusChangedEventHandler(ILogger<RequestStatusChangedEventHandler> logger)
    : IEventHandler<RequestStatusChangedEvent>
{
    public async Task Handle(RequestStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Request {RequestId} status changed from {OldStatus} to {NewStatus} at {Timestamp}",
            notification.RequestId,
            notification.PreviousStatus.Value,
            notification.NewStatus.Value,
            notification.OccurredOn);

        // TODO: Future implementation
        // - Record status change in audit log
        // - Trigger status-specific workflows
        // - Send notifications based on new status

        await Task.CompletedTask;
    }
}

