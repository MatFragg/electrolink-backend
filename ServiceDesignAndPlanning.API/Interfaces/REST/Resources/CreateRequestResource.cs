using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;

/// <summary>
/// Resource received from the client to create a new service request
/// </summary>
public record CreateRequestResource(
    Guid ClientId,
    Guid TechnicianId,
    Guid PropertyId,
    Guid ServiceId,
    string ProblemDescription,
    DateOnly ScheduledDate,
    ElectricBill Bill,
    List<RequestPhotoResource> Photos
);