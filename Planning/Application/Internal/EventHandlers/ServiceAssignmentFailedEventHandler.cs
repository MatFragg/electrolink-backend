using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

/// <summary>
/// Manejador de evento: Cuando falla una asignación automática.
/// Reintenta el matching o marca el request como expirado si se agotan reintentos.
/// Hotspot 5: Manejo de fallos en asignación automática.
/// </summary>
public class ServiceAssignmentFailedEventHandler
{
    private readonly IServiceRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ServiceAssignmentFailedEventHandler> _logger;
    
    private const int MAX_RETRIES = 3;

    public ServiceAssignmentFailedEventHandler(
        IServiceRequestRepository requestRepository,
        IUnitOfWork unitOfWork,
        ILogger<ServiceAssignmentFailedEventHandler> logger)
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
            int retryCount = @event.RetryCount ?? 0;
            string reason = @event.Reason ?? "Unknown reason";

            _logger.LogInformation($"[Planning] Handling ServiceAssignmentFailed for Request {requestId}, Retry {retryCount}");

            var request = await _requestRepository.FindByIdAsync(RequestId.From(requestId));
            if (request == null)
            {
                _logger.LogWarning($"[Planning] Request {requestId} not found");
                return;
            }

            // Si se agotan reintentos, marcar como expirado
            if (retryCount >= MAX_RETRIES)
            {
                _logger.LogWarning($"[Planning] Request {requestId} expired after {MAX_RETRIES} retry attempts");
                request.Expire();
            }
            else
            {
                _logger.LogInformation($"[Planning] Request {requestId} will be retried. Attempt {retryCount + 1}/{MAX_RETRIES}");
                // El request permanece en PendingAssignment para reintentar
            }

            _requestRepository.Update(request);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Planning] Error handling ServiceAssignmentFailed event");
            throw;
        }
    }
}

