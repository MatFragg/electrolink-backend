using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

public class ServiceRequestCreatedEventHandler(ISubscriptionCommandService commandService)
    : INotificationHandler<ServiceRequestCreatedEvent>
{
    public async Task Handle(ServiceRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        await commandService.Handle(new IncrementMonthlyRequestCounterCommand(
            UserId: notification.HomeownerId.Value));
    }
}

