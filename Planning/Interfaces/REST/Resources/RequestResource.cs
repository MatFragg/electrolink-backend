using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;
public record RequestResource(
    string RequestId,
    string ClientId,
    string TechnicianId,
    string PropertyId,
    string ServiceId,
    string ProblemDescription,
    DateOnly ScheduledDate,
    string Status,
    ElectricBill Bill,
    List<RequestPhotoResource> Photos
);