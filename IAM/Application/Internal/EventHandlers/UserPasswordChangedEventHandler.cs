using Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.IAM.Application.Internal.EventHandlers;

public class UserPasswordChangedEventHandler : IEventHandler<UserPasswordChangedEvent>
{
    private readonly ILogger<UserPasswordChangedEventHandler> _logger;

    public UserPasswordChangedEventHandler(ILogger<UserPasswordChangedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserPasswordChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[IAM BC] Domain Event: UserPasswordChangedEvent recibido para usuario ID: {notification.UserId}. Contraseña cambiada.");
        return Task.CompletedTask;
    }
}
