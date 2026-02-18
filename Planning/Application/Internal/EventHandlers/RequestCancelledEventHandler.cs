using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Microsoft.Extensions.Logging;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Handles RequestCancelledEvent for cleanup and notifications.
/// </summary>
public class RequestCancelledEventHandler(ILogger<RequestCancelledEventHandler> logger)
    : IEventHandler<RequestCancelledEvent>
{
    public async Task Handle(RequestCancelledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Request {RequestId} cancelled. Reason: {Reason} at {Timestamp}",
            notification.RequestId,
            notification.CancellationReason,
            notification.OccurredOn);

        // TODO: Future implementation
        // - Free up technician's schedule
        // - Send cancellation notifications
        // - Process cancellation penalties if applicable
        // - Update subscription usage counter

        await Task.CompletedTask;
    }
}

