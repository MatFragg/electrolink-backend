using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

/// <summary>
/// Command to upload a work photo during service execution.
/// </summary>
public record UploadWorkPhotoCommand(
    ServiceExecutionId ExecutionId,
    TechnicianId TechnicianId,
    EPhotoType PhotoType,
    string PhotoUrl,
    string ProviderId,
    string? ThumbnailUrl,
    long SizeBytes,
    string Format,
    DateTime TakenAt,
    string? Notes
);
