namespace Hampcoders.Electrolink.API.ServiceDesignAndPlanning.API.Domain.Model.ValueObjects;

public record ServiceRestriction(
    List<string> UnavailableDistricts,
    List<string> ForbiddenDays,
    bool RequiresSpecialCertification
);