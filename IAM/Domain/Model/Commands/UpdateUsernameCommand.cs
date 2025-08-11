namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;

public record UpdateUsernameCommand(int UserId, string NewUsername);