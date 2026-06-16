using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using MediatR;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.EventHandlers;

public class TechnicianEvaluationSubmittedEventHandler(
    ITechnicianMetricsCommandService technicianMetricsCommandService,
    ILogger<TechnicianEvaluationSubmittedEventHandler> logger)
    : INotificationHandler<TechnicianEvaluationSubmittedIntegrationEvent>
{
    public async Task Handle(TechnicianEvaluationSubmittedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("[Analytics BC] TechnicianEvaluation: tech={TechnicianId}, score={Score}",
            notification.TechnicianId, notification.Score);

        await technicianMetricsCommandService.UpdateTechnicianRatingMetricAsync(
            notification.TechnicianId,
            notification.Score);
    }
}
