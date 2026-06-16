using Hampcoders.Electrolink.API.Analytics.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Analytics.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Analytics.Domain.Repositories;
using Hampcoders.Electrolink.API.Analytics.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using MediatR;

namespace Hampcoders.Electrolink.API.Analytics.Application.Internal.CommandServices;

public class TechnicianMetricsCommandService(
    ITechnicianMetricsRepository metricsRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ILogger<TechnicianMetricsCommandService> logger)
    : ITechnicianMetricsCommandService
{
    public async Task UpdateTechnicianMetricsAsync(
        string technicianId,
        string clientId,
        decimal serviceRevenueAmount,
        string currency,
        TimeSpan responseTime,
        bool requiresIoTCertification)
    {
        var metrics = await metricsRepository.FindCurrentPeriodByTechnicianAsync(
            TechnicianId.From(technicianId));

        if (metrics == null)
        {
            metrics = TechnicianMetrics.InitializeForPeriod(
                TechnicianId.From(technicianId),
                DateRange.CurrentMonth());
            await metricsRepository.AddAsync(metrics);
        }

        metrics.ApplyServiceCompleted(
            clientId,
            Money.Of(serviceRevenueAmount, currency),
            responseTime,
            requiresIoTCertification);

        metricsRepository.Update(metrics);
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in metrics.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        metrics.ClearDomainEvents();

        logger.LogInformation("[Analytics BC] Technician metrics updated for {TechnicianId}", technicianId);
    }

    public async Task UpdateTechnicianRatingMetricAsync(string technicianId, decimal rating)
    {
        var metrics = await metricsRepository.FindCurrentPeriodByTechnicianAsync(
            TechnicianId.From(technicianId))
            ?? throw new InvalidOperationException(
                $"No current period metrics found for technician '{technicianId}'.");

        metrics.ApplyRatingReceived(rating);
        metricsRepository.Update(metrics);
        await unitOfWork.CompleteAsync();
    }

    public async Task InitializeTechnicianMetricsPeriodAsync(string technicianId)
    {
        var period = DateRange.CurrentMonth();
        var existing = await metricsRepository.FindByTechnicianAndPeriodAsync(
            TechnicianId.From(technicianId), period);

        if (existing != null)
            return;

        var metrics = TechnicianMetrics.InitializeForPeriod(
            TechnicianId.From(technicianId), period);

        await metricsRepository.AddAsync(metrics);
        await unitOfWork.CompleteAsync();

        foreach (var domainEvent in metrics.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        metrics.ClearDomainEvents();
    }
}
