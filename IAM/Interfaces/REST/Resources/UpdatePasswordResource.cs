namespace Hampcoders.Electrolink.API.IAM.Interfaces.REST.Resources;

public record UpdatePasswordResource(string CurrentPassword, string NewPassword);