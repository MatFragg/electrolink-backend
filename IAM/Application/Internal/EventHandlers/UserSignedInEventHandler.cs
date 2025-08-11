using Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.IAM.Application.Internal.EventHandlers;


public class UserSignedInEventHandler : IEventHandler<UserSignedInEvent>
{
    private readonly ILogger<UserSignedInEventHandler> _logger;

    public UserSignedInEventHandler(ILogger<UserSignedInEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserSignedInEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[IAM BC] Domain Event: UserSignedInEvent recibido para usuario ID: {notification.UserId}.");
        return Task.CompletedTask;
    }
}