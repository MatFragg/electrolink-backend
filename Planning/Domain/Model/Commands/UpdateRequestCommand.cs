using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

/// <summary>
/// Command to update an existing service request
/// </summary>
public record UpdateRequestCommand(
    string RequestId,
    Guid ClientId,
    Guid TechnicianId,
    Guid PropertyId,
    Guid ServiceId,
    DateOnly ScheduledDate,
    string ProblemDescription,
    ElectricBill Bill,
    List<RequestPhotoResource> Photos
);