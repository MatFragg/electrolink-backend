namespace Hampcoders.Electrolink.API.Planning.API.Domain.Model.ValueObjects;

public record ServiceRestriction(
    List<string> UnavailableDistricts,
    List<string> ForbiddenDays,
    bool RequiresSpecialCertification
);