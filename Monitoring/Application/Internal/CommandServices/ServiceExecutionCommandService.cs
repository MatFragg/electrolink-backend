using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Repositories;
using Hampcoders.Electrolink.API.Monitoring.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.Monitoring.Application.Internal.CommandServices;

public class ServiceExecutionCommandService(
    IServiceExecutionRepository repository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ILogger<ServiceExecutionCommandService> logger)
    : IServiceExecutionCommandService
{
    // ── Triggered por IServiceOperationContextFacade.CreateServiceExecutionAsync ──
    public async Task<ServiceExecution> Handle(CreateServiceExecutionCommand command)
    {
        logger.LogInformation("[SOM BC] Creating ServiceExecution for assignment {AssignmentId}", command.AssignmentId);

        var existing = await repository.FindByAssignmentIdAsync(command.AssignmentId);
        if (existing is not null)
            throw new InvalidOperationException(
                $"A ServiceExecution already exists for assignment {command.AssignmentId}.");

        var execution = ServiceExecution.Create(command);

        await repository.AddAsync(execution);
        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Técnico inicia en campo ──────────────────────────
    public async Task<ServiceExecution> Handle(StartServiceExecutionCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        execution.Start(command.TechnicianId, command.StartedAt);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Técnico sube foto ────────────────────────────────
    public async Task<ServiceExecution> Handle(UploadWorkPhotoCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        if (!Enum.TryParse<EPhotoType>(command.PhotoType, true, out var photoType))
            throw new ArgumentException($"Invalid photo type: {command.PhotoType}");

        string photoId = $"photo-{Guid.NewGuid()}";
        execution.UploadPhoto(photoId, photoType, command.PhotoUrl, command.TakenAt, command.Notes);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Técnico registra componentes ─────────────────────
    public async Task<ServiceExecution> Handle(RecordComponentsUsedCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        var records = command.ComponentsUsed.Select(dto =>
            ComponentUsageRecord.Create(
                ComponentUsageId.NewId(),
                execution.Id,
                dto.ComponentTypeId,
                dto.ComponentTypeName,
                dto.QuantityUsed,
                dto.QuantityReserved)).ToList();

        execution.RecordComponentsUsed(records, command.RecordedAt);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Técnico actualiza reporte técnico ─────────────────
    public async Task<ServiceExecution> Handle(UpdateTechnicalReportCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        execution.UpdateTechnicalReport(
            command.ReportContent, command.Findings,
            command.Recommendations, command.UpdatedAt);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Técnico completa el servicio ──────────────────────
    public async Task<ServiceExecution> Handle(CompleteServiceExecutionCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        execution.Complete(command.TechnicianId, command.CompletedAt);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        logger.LogInformation(
            "[SOM BC] ⭐ Service {AssignmentId} completed by technician {TechnicianId}",
            execution.AssignmentId.Value, command.TechnicianId);

        return execution;
    }

    // ── Cancelación ───────────────────────────────────────
    public async Task<ServiceExecution> Handle(CancelServiceExecutionCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        if (!Enum.TryParse<ECancelledBy>(command.CancelledBy, true, out var cancelledBy))
            throw new ArgumentException($"Invalid cancelledBy value: {command.CancelledBy}");

        if (!Enum.TryParse<ECancellationReason>(command.Reason, true, out var reason))
            throw new ArgumentException($"Invalid cancellation reason: {command.Reason}");

        execution.Cancel(command.ActorId, cancelledBy, reason, command.Notes, command.RequestReassignment);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Propietario extiende el tiempo de espera ──────────
    public async Task<ServiceExecution> Handle(ExtendServiceWaitTimeCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        execution.ExtendWaitTime(command.HomeownerId, command.ExtendMinutes, command.RequestedAt);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Propietario evalúa al técnico ─────────────────────
    public async Task<ServiceExecution> Handle(SubmitHomeownerReviewCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        execution.SubmitHomeownerReview(
            command.ReviewerId, command.Rating,
            command.Comment, command.Categories, command.SubmittedAt);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Técnico evalúa al propietario ─────────────────────
    public async Task<ServiceExecution> Handle(SubmitTechnicianReviewCommand command)
    {
        var execution = await GetOrThrowAsync(command.ExecutionId);

        execution.SubmitTechnicianReview(
            command.ReviewerId, command.Rating,
            command.Comment, command.Categories, command.SubmittedAt);

        await unitOfWork.CompleteAsync();
        await PublishAndClearEventsAsync(execution);

        return execution;
    }

    // ── Helpers ───────────────────────────────────────────
    private async Task<ServiceExecution> GetOrThrowAsync(ServiceExecutionId executionId)
    {
        var execution = await repository.FindByIdAsync(executionId);
        return execution
            ?? throw new KeyNotFoundException($"ServiceExecution {executionId} not found.");
    }

    private async Task PublishAndClearEventsAsync(ServiceExecution execution)
    {
        foreach (var domainEvent in execution.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        execution.ClearDomainEvents();
    }
}