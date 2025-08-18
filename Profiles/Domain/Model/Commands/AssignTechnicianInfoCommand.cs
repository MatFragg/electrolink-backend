namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record AssignTechnicianInfoCommand(
    int ProfileId,
    string LicenseNumber,
    string Specialization);