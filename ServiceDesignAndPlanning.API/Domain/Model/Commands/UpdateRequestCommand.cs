using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.Commands;

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