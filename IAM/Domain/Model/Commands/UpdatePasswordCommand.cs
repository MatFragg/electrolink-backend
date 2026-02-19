namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;

public record UpdatePasswordCommand(string UserId,string CurrentPassword, string NewPassword);