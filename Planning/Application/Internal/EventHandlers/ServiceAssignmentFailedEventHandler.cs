using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Events;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Application.Internal.EventHandler;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;

namespace Hampcoders.Electrolink.API.Planning.Application.Internal.EventHandlers;

public class ServiceAssignmentFailedEventHandler(
    IServiceRequestRepository requestRepository,
    IUnitOfWork unitOfWork,
    ILogger<ServiceAssignmentFailedEventHandler> logger)
    : IEventHandler<ServiceAssignmentFailedEvent>
{
    private const int MaxRetries = 3;

    public async Task Handle(ServiceAssignmentFailedEvent @event, CancellationToken cancellationToken)
    {
        var requestId = @event.RequestId.Value;
        var retryCount = @event.RetryCount;
        var reason = @event.FailureReason;

        logger.LogInformation(
            "[Planning] Handling ServiceAssignmentFailed for Request {RequestId}, Retry {RetryCount}",
            requestId, retryCount);

        var request = await requestRepository.FindByIdAsync(@event.RequestId);
        if (request is null)
        {
            logger.LogWarning("[Planning] Request {RequestId} not found", requestId);
            return;
        }

        if (retryCount >= MaxRetries)
        {
            logger.LogWarning(
                "[Planning] Request {RequestId} expired after {MaxRetries} retry attempts",
                requestId, MaxRetries);
            request.Expire();
        }
        else
        {
            logger.LogInformation(
                "[Planning] Request {RequestId} will be retried. Attempt {Attempt}/{MaxRetries}",
                requestId, retryCount + 1, MaxRetries);
        }

        requestRepository.Update(request);
        await unitOfWork.CompleteAsync();
    }
}
