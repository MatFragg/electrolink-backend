namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ServiceRestriction(
    List<string> UnavailableDistricts,
    List<string> ForbiddenDays,
    bool RequiresSpecialCertification
);