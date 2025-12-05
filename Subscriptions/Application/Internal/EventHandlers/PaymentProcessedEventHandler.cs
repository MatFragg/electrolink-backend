using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

/// <summary>
/// Handles the <see cref="PaymentProcessedEvent"/> domain event.
/// </summary>
public class PaymentProcessedEventHandler : IEventHandler<PaymentProcessedEvent>
{
    private readonly ILogger<PaymentProcessedEventHandler> _logger;

    public PaymentProcessedEventHandler(ILogger<PaymentProcessedEventHandler> logger)
    {
        _logger = logger;
        _logger.LogInformation("[PaymentProcessedEventHandler CTOR] Instanciando PaymentProcessedEventHandler.");
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentProcessedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Subscriptions BC] Domain Event: PaymentProcessedEvent recibido para Transacción: {notification.TransactionId}, Estado: {notification.Status}.");
        // Aquí podrías desencadenar notificaciones al usuario, actualizar registros de facturación, etc.
        await Task.CompletedTask;
    }
}