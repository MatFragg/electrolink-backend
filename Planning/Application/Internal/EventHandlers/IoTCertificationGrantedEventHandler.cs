using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Policy: UpdateCatalogIoTEligibility
/// WHEN IoTCertificationGranted (from Profiles BC)
/// THEN el técnico queda habilitado para crear recipes IoT.
///      No se modifican automáticamente los recipes existentes.
///      La certificación se consulta en tiempo real en ServiceCatalogCommandService.
/// </summary>
public class IoTCertificationGrantedEventHandler(
    ILogger<IoTCertificationGrantedEventHandler> logger)
    : IEventHandler<IoTCertificationGrantedEvent>
{
    public Task Handle(IoTCertificationGrantedEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[Planning] IoTCertificationGranted for technician {TechnicianId}. " +
            "They can now create IoT recipes.",
            @event.TechnicianId);

        return Task.CompletedTask;
    }
}

/// <summary>
/// Integration event from Profiles BC — certificación IoT otorgada a un técnico.
/// </summary>
public record IoTCertificationGrantedEvent(string TechnicianId, DateTime OccurredOn) : IEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
