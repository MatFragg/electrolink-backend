using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class PortfolioItemRemovedEventHandler(ILogger<PortfolioItemRemovedEventHandler> logger) : IEventHandler<PortfolioItemRemovedEvent>
{

    public async Task Handle(PortfolioItemRemovedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Profiles BC] Domain Event: PortfolioItemRemovedEvent recibido para ProfileId: {notification.ProfileId}, WorkId: {notification.WorkId}.");
        /*var newNotification = new UserNotification(
            new UserId(notification.ProfileId),
            $"El ítem de portafolio con ID '{notification.WorkId}' ha sido eliminado.", 
            $"/profile/{notification.ProfileId}/portfolio"
        );
        await _userNotificationRepository.AddNotificationAsync(newNotification);
        _logger.LogInformation($"[Profiles BC] Notificación de ítem de portafolio eliminado creada para ProfileId: {notification.ProfileId}.");*/
    }
}