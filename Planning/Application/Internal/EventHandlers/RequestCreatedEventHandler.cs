using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Microsoft.Extensions.Logging;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Handles RequestCreatedEvent for internal processing like logging and analytics.
/// </summary>
public class RequestCreatedEventHandler(ILogger<RequestCreatedEventHandler> logger) 
    : IEventHandler<RequestCreatedEvent>
{
    public async Task Handle(RequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Request created: {RequestId} for Client: {ClientId} at {Timestamp}",
            notification.RequestId,
            notification.ClientId,
            notification.OccurredOn);

        // TODO: Future implementation
        // - Send notification to Monitoring BC via Integration Event
        // - Trigger automatic technician assignment logic
        // - Record analytics metrics

        await Task.CompletedTask;
    }
}

