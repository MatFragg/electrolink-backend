namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;

public record UpdateUsernameCommand(string UserId, string NewUsername);