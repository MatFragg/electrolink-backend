namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;

public record UpdatePasswordCommand(int UserId, string NewPassword);