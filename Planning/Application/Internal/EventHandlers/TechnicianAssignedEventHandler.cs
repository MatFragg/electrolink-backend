using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Microsoft.Extensions.Logging;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Handles TechnicianAssignedEvent for notifications and workflow triggers.
/// </summary>
public class TechnicianAssignedEventHandler(ILogger<TechnicianAssignedEventHandler> logger)
    : IEventHandler<TechnicianAssignedEvent>
{
    public async Task Handle(TechnicianAssignedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Technician {TechnicianId} assigned to Request {RequestId} at {Timestamp}",
            notification.TechnicianId,
            notification.RequestId,
            notification.OccurredOn);

        // TODO: Future implementation
        // - Send notification to assigned technician
        // - Update technician's schedule
        // - Notify client of assignment

        await Task.CompletedTask;
    }
}

