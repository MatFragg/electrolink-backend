using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record RegisterWorkPhotoCommand(
    ServiceExecutionId ExecutionId,
    TechnicianId TechnicianId,
    string ProviderId,
    string PublicUrl,
    string? ThumbnailUrl,
    EPhotoType PhotoType,
    string Format,
    long SizeBytes,
    DateTime TakenAt,
    string? Notes
);
