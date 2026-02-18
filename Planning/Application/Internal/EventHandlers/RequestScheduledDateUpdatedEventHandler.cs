using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Microsoft.Extensions.Logging;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Handles RequestScheduledDateUpdatedEvent for schedule management.
/// </summary>
public class RequestScheduledDateUpdatedEventHandler(ILogger<RequestScheduledDateUpdatedEventHandler> logger)
    : IEventHandler<RequestScheduledDateUpdatedEvent>
{
    public async Task Handle(RequestScheduledDateUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Request {RequestId} scheduled date updated from {OldDate} to {NewDate} at {Timestamp}",
            notification.RequestId,
            notification.PreviousDate,
            notification.NewDate,
            notification.OccurredOn);

        // TODO: Future implementation
        // - Update technician's calendar
        // - Send rescheduling notification to client and technician
        // - Check for schedule conflicts

        await Task.CompletedTask;
    }
}

