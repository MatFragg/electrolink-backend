namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record UpdateTechnicianCoverageCommand(
    int ProfileId,
    string NewCoverageAreaDetails
);