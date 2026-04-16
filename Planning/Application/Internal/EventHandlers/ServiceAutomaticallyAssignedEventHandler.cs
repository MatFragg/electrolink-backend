using Hampcoders.Electrolink.API.Planning.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Manejador de evento: Cuando una asignación se ejecuta exitosamente.
/// Marca el request como Assigned.
/// Hotspot 4: Reactividad automática a eventos de asignación.
/// </summary>
public class ServiceAutomaticallyAssignedEventHandler(
    IServiceDesignQueryService serviceDesignQueryService,
    IServiceRequestCommandService requestCommandService,
    ExternalMonitoringService serviceOperationFacade,
    ILogger<ServiceAutomaticallyAssignedEventHandler> logger)
    : INotificationHandler<ServiceAutomaticallyAssignedEvent>
{
    public async Task Handle(
        ServiceAutomaticallyAssignedEvent notification,
        CancellationToken cancellationToken)
    {
        var request = await serviceDesignQueryService.Handle(new GetServiceRequestByIdQuery(notification.RequestId));
        
        if (request is null)
        {
            logger.LogError("[Planning BC] Original ServiceRequest {RequestId} not found. Cannot trigger SOM execution.", notification.RequestId.Value);
            return;
        }

        try
        {
            await serviceOperationFacade.CreateServiceExecutionAsync(
                notification.AssignmentId.Value,
                notification.RequestId.Value,
                notification.TechnicianId.Value,
                request.HomeownerId.Value,
                request.PropertyId.Value,
                notification.RecipeSnapshot,
                notification.ScheduledAt,
                notification.IsPriority);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create ServiceExecution");
        }
    }
}