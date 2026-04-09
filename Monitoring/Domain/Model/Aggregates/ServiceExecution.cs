using System.ComponentModel.DataAnnotations;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Events;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

/// <summary>
/// ServiceExecution Aggregate Root represents the execution of a scheduled service assignment.
/// Tracks the entire lifecycle from scheduling through evaluation.
/// </summary>
public class ServiceExecution : BaseAggregateRoot
{
    public ServiceExecutionId Id { get; private set; } = null!;
    public AssignmentId AssignmentId { get; private set; } = null!;
    public RequestId RequestId { get; private set; } = null!;
    public TechnicianId TechnicianId { get; private set; } = null!;
    public HomeownerId HomeownerId { get; private set; } = null!;
    public PropertyId PropertyId { get; private set; } = null!;
    public RecipeSnapshot RecipeSnapshot { get; private set; } = null!;
    public EExecutionStatus Status { get; private set; }
    public DateTime ScheduledDateTime { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    private readonly List<WorkPhoto> _workPhotos = new();
    public IReadOnlyCollection<WorkPhoto> WorkPhotos => _workPhotos.AsReadOnly();
    private readonly List<ComponentUsageRecord> _componentSubstitutions = new();
    public IReadOnlyCollection<ComponentUsageRecord> ComponentSubstitutions => _componentSubstitutions.AsReadOnly();
    [MaxLength(255)]
    public string? TechnicalReportContent { get; private set; }
    [MaxLength(255)]
    public string? TechnicalReportFindings { get; private set; }
    [MaxLength(255)]
    public string? TechnicalReportRecommendations { get; private set; }
    public int TechnicalReportVersion { get; private set; }
    public bool IsPriority { get; private set; }
    public CancellationRequestId? CancellationRequestId { get; private set; }
    public EvaluationId? HomeownerEvaluationId { get; private set; }
    public EvaluationId? TechnicianEvaluationId { get; private set; }
    private readonly List<ServiceEvaluation> _evaluations = new();
    public IReadOnlyCollection<ServiceEvaluation> Evaluations => _evaluations.AsReadOnly();
    public DateTime? NoShowDetectedAt { get; private set; }
    public DateTime? WaitExtendedUntil { get; private set; }
    public DateTime? EvaluationWindowExpiresAt { get; private set; }
    public bool EvaluationWindowExpired { get; private set; }

    protected ServiceExecution() { }

    /// <summary>
    /// Factory method to create a new ServiceExecution from an assignment.
    /// </summary>
    public static ServiceExecution Create(CreateServiceExecutionCommand command)
    {
        if (command.AssignmentId == null) throw new ArgumentNullException(nameof(AssignmentId));
        if (command.RequestId == null) throw new ArgumentNullException(nameof(RequestId));
        if (command.TechnicianId == null) throw new ArgumentNullException(nameof(TechnicianId));
        if (command.HomeownerId == null) throw new ArgumentNullException(nameof(HomeownerId));
        if (command.PropertyId == null) throw new ArgumentNullException(nameof(PropertyId));
        if (command.RecipeSnapshot == null) throw new ArgumentNullException(nameof(RecipeSnapshot));

        var exec = new ServiceExecution
        {
            Id = ServiceExecutionId.NewId(),
            AssignmentId = command.AssignmentId,
            RequestId = command.RequestId,
            TechnicianId = command.TechnicianId,
            HomeownerId = command.HomeownerId,
            PropertyId = command.PropertyId,
            RecipeSnapshot = command.RecipeSnapshot,
            ScheduledDateTime = command.ScheduledDateTime,
            Status = EExecutionStatus.Scheduled,
            IsPriority = command.IsPriority,
            TechnicalReportVersion = 0
        };

        return exec;
    }

    /// <summary>
    /// Starts the service execution.
    /// Transitions status from Scheduled to InProgress.
    /// </summary>
    public void Start(TechnicianId technicianId, DateTime startedAt)
    {
        EnsureStatus(EExecutionStatus.Scheduled);
        EnsureTechnicianOwnership(technicianId);

        Status = EExecutionStatus.InProgress;
        StartedAt = startedAt;

        RaiseDomainEvent(new ServiceExecutionStartedEvent(
            Id, AssignmentId,
            TechnicianId, HomeownerId,
            EExecutionStatus.Scheduled, EExecutionStatus.InProgress,
            startedAt, new DateTime()));
    }

    /// <summary>
    /// Uploads a work photo during the service execution.
    /// </summary>
    public void UploadPhoto(EPhotoType photoType, string photoUrl, DateTime takenAt, string? notes)
    {
        EnsureStatus(EExecutionStatus.InProgress);

        if (string.IsNullOrWhiteSpace(photoUrl))
            throw new ArgumentException("Photo URL cannot be empty.", nameof(photoUrl));

        var photo = WorkPhoto.Create(Id, photoType, photoUrl, takenAt, notes);
        _workPhotos.Add(photo);

        RaiseDomainEvent(new WorkPhotoUploadedEvent(
            Id, photo.Id, photoType, DateTime.UtcNow));
    }

    /// <summary>
    /// Records actual component usage during the service.
    /// </summary>
    public void RecordComponentsUsed(IReadOnlyList<ComponentUsageRecord> components, DateTime recordedAt)
    {
        EnsureStatus(EExecutionStatus.InProgress);

        if (components is null || components.Count == 0)
            throw new ArgumentException("At least one component must be recorded.");

        _componentSubstitutions.Clear();
        foreach (var c in components)
            _componentSubstitutions.Add(c);

        bool hasOverage = _componentSubstitutions.Any(c => c.Delta > 0);

        RaiseDomainEvent(new ComponentsActuallyUsedRecordedEvent(
            Id, AssignmentId,
            _componentSubstitutions.Select(c => new ComponentUsage(
                c.ComponentTypeId, c.ComponentTypeName,
                c.QuantityUsed, c.QuantityReserved, c.Delta)).ToList(),
            hasOverage,
            recordedAt, DateTime.UtcNow));
    }

    /// <summary>
    /// Updates the technical report with findings and recommendations.
    /// </summary>
    public void UpdateTechnicalReport(string content, string findings, string recommendations, DateTime updatedAt)
    {
        EnsureStatus(EExecutionStatus.InProgress);

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Report content cannot be empty.", nameof(content));

        TechnicalReportContent         = content;
        TechnicalReportFindings        = findings;
        TechnicalReportRecommendations = recommendations;
        TechnicalReportVersion++;

        RaiseDomainEvent(new TechnicalReportUpdatedEvent(
            Id, TechnicalReportVersion, updatedAt));
    }

    /// <summary>
    /// Completes the service execution.
    /// Transitions status from InProgress to Completed.
    /// Opens evaluation window for both homeowner and technician.
    /// </summary>
    public void Complete(TechnicianId technicianId, DateTime completedAt)
    {
        EnsureStatus(EExecutionStatus.InProgress);
        EnsureTechnicianOwnership(technicianId);

        if (_workPhotos.Count == 0)
            throw new InvalidOperationException("At least one work photo is required to complete the service.");

        if (string.IsNullOrWhiteSpace(TechnicalReportContent))
            throw new InvalidOperationException("A technical report is required to complete the service.");

        if (_componentSubstitutions.Count == 0)
            throw new InvalidOperationException("At least one component record is required to complete the service.");

        Status = EExecutionStatus.Completed;
        CompletedAt = completedAt;

        bool hasOverage = _componentSubstitutions.Any(c => c.Delta > 0);

        RaiseDomainEvent(new ServiceCompletedEvent(
            Id,
            AssignmentId,
            RequestId,
            TechnicianId,
            HomeownerId,
            PropertyId,
            RecipeSnapshot.ServiceCategory.ToString(),
            _componentSubstitutions.Select(c => new ComponentUsage(
                c.ComponentTypeId, c.ComponentTypeName,
                c.QuantityUsed, c.QuantityReserved, c.Delta)).ToList(),
            hasOverage,
            _workPhotos.Select(p => p.PhotoUrl).ToList(),
            TechnicalReportContent,
            completedAt, DateTime.UtcNow));
    }

    /// <summary>
    /// Cancels the service execution.
    /// Can only be cancelled if not already completed or cancelled.
    /// </summary>
    public void Cancel(
        string cancelledById,
        ECancelledBy cancelledBy,
        ECancellationReason reason,
        string? notes,
        bool requestReassignment)
    {
        if (Status == EExecutionStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed service.");
        if (Status == EExecutionStatus.Cancelled)
            throw new InvalidOperationException("Service is already cancelled.");

        // Si el técnico cancela, la reasignación no aplica
        bool effectiveReassignment = cancelledBy == ECancelledBy.Technician ? false : requestReassignment;

        Status = EExecutionStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;

        RaiseDomainEvent(new ServiceExecutionCancelledEvent(
            Id,
            AssignmentId,
            RequestId,
            TechnicianId,
            HomeownerId,
            cancelledBy,
            reason.ToString(),
            notes,
            Status.ToString(),
            effectiveReassignment,
            CancelledAt.Value,
            DateTime.UtcNow));
    }
    
    public void OpenEvaluationWindow()
    {
        if (Status != EExecutionStatus.Completed)
            throw new InvalidOperationException("Evaluation window can only be opened for completed services.");

        EvaluationWindowExpiresAt = DateTime.UtcNow.AddDays(7);
        EvaluationWindowExpired = false;

        RaiseDomainEvent(new EvaluationWindowOpenedEvent(
            Id,
            AssignmentId,
            TechnicianId,
            HomeownerId,
            EvaluationWindowExpiresAt.Value,
            DateTime.UtcNow,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Records a homeowner review/evaluation.
    /// </summary>
    public void SubmitHomeownerReview(HomeownerId homeownerId, int rating, string? comment, Dictionary<string, int> categories, DateTime submittedAt)
    {
        EnsureStatus(EExecutionStatus.Completed);
        EnsureHomeownerOwnership(homeownerId);
        EnsureEvaluationWindowOpen();

        if (_evaluations.Any(e => e.ReviewerId == homeownerId.Value && e.ReviewerRole == "Homeowner"))
            throw new InvalidOperationException("Homeowner has already submitted a review for this service.");

        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        var eval = ServiceEvaluation.Create(Id, homeownerId.Value, TechnicianId.Value, "Homeowner", rating, comment, categories, submittedAt);
        _evaluations.Add(eval);

        RaiseDomainEvent(new HomeownerReviewSubmittedEvent(
            Id, AssignmentId,
            homeownerId.Value, TechnicianId.Value,
            rating, categories, submittedAt, DateTime.UtcNow));
    }

    /// <summary>
    /// Records a technician review/evaluation.
    /// </summary>
    public void SubmitTechnicianReview(TechnicianId technicianId, int rating, string? comment, Dictionary<string, int> categories, DateTime submittedAt)
    {
        EnsureStatus(EExecutionStatus.Completed);
        EnsureTechnicianOwnership(technicianId);
        EnsureEvaluationWindowOpen();

        if (_evaluations.Any(e => e.ReviewerId == technicianId.Value && e.ReviewerRole == "Technician"))
            throw new InvalidOperationException("Technician has already submitted a review for this service.");

        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        var eval = ServiceEvaluation.Create(Id, technicianId.Value, HomeownerId.Value, "Technician", rating, comment, categories, submittedAt);
        _evaluations.Add(eval);

        RaiseDomainEvent(new TechnicianReviewSubmittedEvent(
            Id, AssignmentId,
            technicianId.Value, HomeownerId.Value,
            rating, categories, submittedAt, DateTime.UtcNow));
    }

    /// <summary>
    /// Closes the evaluation window.
    /// </summary>
    public void ExpireEvaluationWindow()
    {
        if (EvaluationWindowExpired)
            return;

        EvaluationWindowExpired = true;

        bool homeownerEvaluated  = _evaluations.Any(e => e.ReviewerRole == "Homeowner");
        bool technicianEvaluated = _evaluations.Any(e => e.ReviewerRole == "Technician");

        RaiseDomainEvent(new EvaluationWindowExpiredEvent(
            Id, AssignmentId,
            homeownerEvaluated, technicianEvaluated,
            DateTime.UtcNow, DateTime.UtcNow));
    }

    /// <summary>
    /// Records a no-show incident for the technician.
    /// Used when technician fails to appear for the scheduled service.
    /// </summary>
    public void RecordNoShow(DateTime detectedAt, int minutesLate)
    {
        EnsureStatus(EExecutionStatus.Scheduled);

        NoShowDetectedAt = detectedAt;

        RaiseDomainEvent(new TechnicianNoShowDetectedEvent(
            Id, AssignmentId,
            TechnicianId, HomeownerId,
            ScheduledDateTime, detectedAt, minutesLate, DateTime.UtcNow));
    }

    /// <summary>
    /// Extends the wait time for the service.
    /// Used when the technician needs more time to arrive.
    /// </summary>
    public void ExtendWaitTime(HomeownerId homeownerId, int extendMinutes, DateTime requestedAt)
    {
        EnsureStatus(EExecutionStatus.Scheduled);
        EnsureHomeownerOwnership(homeownerId);

        if (extendMinutes <= 0)
            throw new ArgumentException("Extension minutes must be positive.", nameof(extendMinutes));

        WaitExtendedUntil = (NoShowDetectedAt ?? requestedAt).AddMinutes(extendMinutes);

        RaiseDomainEvent(new ServiceWaitTimeExtendedEvent(
            Id, homeownerId,
            extendMinutes, WaitExtendedUntil.Value, requestedAt, DateTime.UtcNow));
    }
    
    private void EnsureStatus(EExecutionStatus expected)
    {
        if (Status != expected)
            throw new InvalidOperationException(
                $"ServiceExecution {Id.Value} must be in status {expected} but is {Status}.");
    }

    private void EnsureTechnicianOwnership(TechnicianId technicianId)
    {
        if (TechnicianId != technicianId)
            throw new UnauthorizedAccessException(
                $"Technician {technicianId} is not assigned to this service execution.");
    }

    private void EnsureHomeownerOwnership(HomeownerId homeownerId)
    {
        if (HomeownerId != homeownerId)
            throw new UnauthorizedAccessException(
                $"Homeowner {homeownerId} is not the owner of this service execution.");
    }

    private void EnsureEvaluationWindowOpen()
    {
        if (EvaluationWindowExpired || EvaluationWindowExpiresAt is null)
            throw new InvalidOperationException("The evaluation window has expired or has not been opened.");

        if (DateTime.UtcNow > EvaluationWindowExpiresAt.Value)
            throw new InvalidOperationException("The evaluation window has expired.");
    }
}
