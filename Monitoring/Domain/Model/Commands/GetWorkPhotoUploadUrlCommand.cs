using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

public record GetWorkPhotoUploadUrlCommand(
    ServiceExecutionId ExecutionId,
    TechnicianId TechnicianId,
    EPhotoType PhotoType
);
