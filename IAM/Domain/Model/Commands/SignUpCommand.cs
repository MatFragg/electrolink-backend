namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;

/**
 * <summary>
 *     The sign up command
 * </summary>
 * <remarks>
 *     This command object includes the email and password to sign up
 * </remarks>
 */
public record SignUpCommand(string Email, string Password, string PasswordConfirmation);