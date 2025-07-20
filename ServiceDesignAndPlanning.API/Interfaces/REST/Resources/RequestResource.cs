namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;
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