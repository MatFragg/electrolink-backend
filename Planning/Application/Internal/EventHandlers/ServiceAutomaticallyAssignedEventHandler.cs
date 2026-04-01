using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Manejador de evento: Cuando una asignación se ejecuta exitosamente.
/// Marca el request como Assigned.
/// Hotspot 4: Reactividad automática a eventos de asignación.
/// </summary>
public class ServiceAutomaticallyAssignedEventHandler(
    IServiceRequestRepository requestRepository,
    ExternalMonitoringService serviceOperationFacade,
    IUnitOfWork unitOfWork,
    ILogger<ServiceAutomaticallyAssignedEventHandler> logger)
    : INotificationHandler<ServiceAutomaticallyAssignedEvent>
{
    public async Task Handle(ServiceAutomaticallyAssignedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var requestId = notification.RequestId;

            logger.LogInformation($"[Planning] Handling ServiceAutomaticallyAssigned for Request {requestId}");

            var request = await requestRepository.FindByIdAsync(requestId);
            if (request == null)
            {
                logger.LogWarning($"[Planning] Request {requestId} not found");
                return;
            }

            requestRepository.Update(request);
            await unitOfWork.CompleteAsync();

            logger.LogInformation($"[Planning] Request {requestId} marked as Assigned");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[Planning] Error handling ServiceAutomaticallyAssigned event");
            throw;
        }
    }
}