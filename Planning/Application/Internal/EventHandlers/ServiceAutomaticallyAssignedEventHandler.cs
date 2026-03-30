using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Manejador de evento: Cuando una asignación se ejecuta exitosamente.
/// Marca el request como Assigned.
/// Hotspot 4: Reactividad automática a eventos de asignación.
/// </summary>
public class ServiceAutomaticallyAssignedEventHandler
{
    private readonly IServiceRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ServiceAutomaticallyAssignedEventHandler> _logger;

    public ServiceAutomaticallyAssignedEventHandler(
        IServiceRequestRepository requestRepository,
        IUnitOfWork unitOfWork,
        ILogger<ServiceAutomaticallyAssignedEventHandler> logger)
    {
        _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(dynamic @event)
    {
        try
        {
            string requestId = @event.RequestId;
            string serviceId = @event.ServiceId;

            _logger.LogInformation($"[Planning] Handling ServiceAutomaticallyAssigned for Request {requestId}");

            var request = await _requestRepository.FindByIdAsync(RequestId.From(requestId));
            if (request == null)
            {
                _logger.LogWarning($"[Planning] Request {requestId} not found");
                return;
            }

            _requestRepository.Update(request);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"[Planning] Request {requestId} marked as Assigned");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Planning] Error handling ServiceAutomaticallyAssigned event");
            throw;
        }
    }
}

