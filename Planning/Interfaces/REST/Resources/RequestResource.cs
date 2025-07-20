using Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.API.Interfaces.REST.Resources;
using Planning.API.Domain.Model.ValueObjects;
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