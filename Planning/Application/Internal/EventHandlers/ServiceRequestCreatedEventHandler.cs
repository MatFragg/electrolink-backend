using Hampcoders.Electrolink.API.Planning.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Event Handler que implementa la policy de auto-asignación automática
/// cuando se confirma una solicitud de servicio
/// </summary>
public class ServiceRequestCreatedEventHandler : IEventHandler<ServiceRequestCreatedEvent>
{
    private readonly IServiceAssignmentCommandService _assignmentCommandService;
    private readonly ILogger<ServiceRequestCreatedEventHandler> _logger;
    
    public ServiceRequestCreatedEventHandler(
        IServiceAssignmentCommandService assignmentCommandService,
        ILogger<ServiceRequestCreatedEventHandler> logger)
    {
        _assignmentCommandService = assignmentCommandService;
        _logger = logger;
    }
    
    public async Task Handle(ServiceRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[Planning BC] Domain Event: ServiceRequestCreated received for request {notification.RequestId}");
        
        // Auto-assign service
        try
        {
            var assignCommand = new AssignServiceToTechnicianCommand(
                notification.RequestId,
                notification.SelectedTechnicianId,
                notification.SelectedRecipeId,
                notification.IsPriority
            );
            
            await _assignmentCommandService.Handle(assignCommand);
            
            _logger.LogInformation($"[Planning BC] Auto-assignment completed successfully for request {notification.RequestId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Planning BC] Failed to auto-assign request {notification.RequestId}");
            // TODO: Publicar ServiceAssignmentFailedEvent
        }
    }
}

