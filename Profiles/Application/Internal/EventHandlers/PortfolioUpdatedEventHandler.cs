using Hampcoders.Electrolink.API.Profiles.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Profiles.Application.Internal.EventHandlers;

public class PortfolioUpdatedEventHandler : IEventHandler<PortfolioUpdatedEvent>
{
    private readonly ILogger<PortfolioUpdatedEventHandler> _logger;
    //private readonly IUserNotificationRepository _userNotificationRepository;

    public PortfolioUpdatedEventHandler(ILogger<PortfolioUpdatedEventHandler> logger /*IUserNotificationRepository userNotificationRepository*/)
    {
        _logger = logger;
        //_userNotificationRepository = userNotificationRepository;
    }

    public async Task Handle(PortfolioUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Profiles BC] Domain Event: PortfolioUpdatedEvent recibido para Técnico: {notification.TechnicianId}.");
        // Una notificación más general si se actualiza el portafolio (ej. "Tu portafolio ha sido modificado")
        /*var newNotification = new UserNotification(
            new UserId(notification.TechnicianId.Id),
            "Tu portafolio de trabajos ha sido actualizado. ¡Revisa los cambios!",
            $"/profile/{notification.TechnicianId.Id}/portfolio"
        );
        await _userNotificationRepository.AddNotificationAsync(newNotification);
        _logger.LogInformation($"[Profiles BC] Notificación general de portafolio actualizada para Técnico: {notification.TechnicianId.Id}.");*/
    }
}