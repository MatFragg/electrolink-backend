using Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.Commands;

/// <summary>
/// Command to create a new service request
/// </summary>
public record CreateRequestCommand(
    Guid ClientId,
    Guid TechnicianId,
    Guid PropertyId,
    Guid ServiceId,
    string Status,
    DateOnly ScheduledDate,
    string ProblemDescription,
    ElectricBill Bill
);