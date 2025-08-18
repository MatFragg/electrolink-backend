namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;


public record UpdateTechnicianSpecialtiesCommand(
    int ProfileId,
    IReadOnlyList<string> NewSpecialties
);