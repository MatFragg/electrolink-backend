using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="PlanCreatedEvent"/> domain event.
/// </summary>
public class PlanCreatedEventHandler : IEventHandler<PlanCreatedEvent>
{
    private readonly ILogger<PlanCreatedEventHandler> _logger;

    public PlanCreatedEventHandler(ILogger<PlanCreatedEventHandler> logger)
    {
        _logger = logger;
        _logger.LogInformation("[PlanCreatedEventHandler CTOR] Instanciando PlanCreatedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(PlanCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: PlanCreatedEvent recibido para Plan: {notification.PlanId}, Nombre: {notification.Name}.");
        // Aquí podrías, por ejemplo, registrar el nuevo plan en un sistema de marketing o analíticas.
        await Task.CompletedTask;
    }
}