using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

/// <summary>
/// Command to update an existing service request
/// </summary>
public record UpdateRequestCommand(
    RequestId RequestId,
    DateOnly? ScheduledDate,
    TechnicianId? TechnicianId,
    string? ProblemDescription
);
