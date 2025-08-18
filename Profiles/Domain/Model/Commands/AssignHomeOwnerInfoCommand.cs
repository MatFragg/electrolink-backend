namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Commands;

public record AssignHomeOwnerInfoCommand(
    int ProfileId,
    string Dni
    );