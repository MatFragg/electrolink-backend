using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

/// <summary>
/// Command to update an existing service request
/// </summary>
public record UpdateRequestCommand(
    Guid RequestId,
    DateOnly? ScheduledDate,
    Guid? TechnicianId,
    string? ProblemDescription
);