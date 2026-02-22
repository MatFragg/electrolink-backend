using Hampcoders.Electrolink.API.IAM.Domain.Model.Events;
using Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Profiles.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class UserRegisteredEventHandler(
    IProfileCommandService profileCommandService,
    ILogger<UserRegisteredEventHandler> logger)
    : INotificationHandler<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Profiles BC] Recibido UserRegisteredEvent para UserId {UserId}",
            notification.UserId);

        var command = new CreateProfileCommand(notification.UserId);

        await profileCommandService.Handle(command);
    }
}