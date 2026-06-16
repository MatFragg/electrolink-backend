using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;
using MediatR;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.EventHandlers;

public class ClientReviewSubmittedEventHandler(ILogger<ClientReviewSubmittedEventHandler> logger)
    : INotificationHandler<ClientReviewSubmittedEvent>
{
    public async Task Handle(ClientReviewSubmittedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Monitoring BC] Client review submitted for service {ExecutionId}. Rating: {Rating}.",
            notification.ExecutionId.Value, notification.Rating);
        await Task.CompletedTask;
    }
}
