using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

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