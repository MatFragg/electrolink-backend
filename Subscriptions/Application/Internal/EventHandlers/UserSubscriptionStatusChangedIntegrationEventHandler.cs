using Hampcoders.Electrolink.API.IAM.Interfaces.ACL;
using Hampcoders.Electrolink.API.Profiles.Interfaces.ACL;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Integration;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="UserSubscriptionStatusChangedIntegrationEvent"/> to update user status in other Bounded Contexts.
/// </summary>
/*public class UserSubscriptionStatusChangedIntegrationEventHandler : IEventHandler<UserSubscriptionStatusChangedIntegrationEvent>
{
    private readonly ILogger<UserSubscriptionStatusChangedIntegrationEventHandler> _logger;
    private readonly IIamContextFacade _iamContextFacade;
    private readonly IProfilesContextFacade _profilesContextFacade;

    public UserSubscriptionStatusChangedIntegrationEventHandler(
        ILogger<UserSubscriptionStatusChangedIntegrationEventHandler> logger,
        IIamContextFacade iamContextFacade,
        IProfilesContextFacade profilesContextFacade)
    {
        _logger = logger;
        _iamContextFacade = iamContextFacade;
        _profilesContextFacade = profilesContextFacade;
        _logger.LogInformation("[UserSubscriptionStatusChangedIntegrationEventHandler CTOR] Instanciando UserSubscriptionStatusChangedIntegrationEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(UserSubscriptionStatusChangedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Integration Event: UserSubscriptionStatusChangedIntegrationEvent recibido para Usuario ID: {notification.UserId}, Nuevo Estado: {notification.NewSubscriptionStatus}.");

        // Update user roles/permissions in IAM
        await _iamContextFacade.UpdateUserRolesAndPermissions(
            notification.UserId,
            notification.NewSubscriptionStatus,
            notification.IsPremium,
            notification.IsCertified);
        _logger.LogInformation($"[Subscriptions BC] IAM ACL: Notificado actualización de roles/permisos para Usuario: {notification.UserId}.");

        // Update user profile information in Profiles
        await _profilesContextFacade.UpdateUserProfileStatus(
            notification.UserId,
            notification.IsCertified,
            notification.CanUseBoost);
        _logger.LogInformation($"[Subscriptions BC] Profiles ACL: Notificado actualización de estado de perfil para Usuario: {notification.UserId}.");

        await Task.CompletedTask;
    }
}*/
