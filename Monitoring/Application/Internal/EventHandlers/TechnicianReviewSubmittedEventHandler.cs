using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.EventHandlers;

public class TechnicianReviewSubmittedEventHandler(ILogger<TechnicianReviewSubmittedEventHandler> logger)
    : INotificationHandler<TechnicianReviewSubmittedEvent>
{
    public async Task Handle(TechnicianReviewSubmittedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Monitoring BC] Technician review submitted for service {ExecutionId}. Rating: {Rating}.",
            notification.ExecutionId.Value, notification.Rating);
        await Task.CompletedTask;
    }
}
