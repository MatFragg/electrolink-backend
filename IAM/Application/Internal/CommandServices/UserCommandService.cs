using Hampcoders.Electrolink.API.IAM.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;
using Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.IAM.Domain.Repositories;
using Hampcoders.Electrolink.API.IAM.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using MediatR;

namespace Hampcoders.Electrolink.API.IAM.Application.Internal.CommandServices;

/**
 * <summary>
 *     The user command service
 * </summary>
 * <remarks>
 *     This class is used to handle user commands
 * </remarks>
 */
public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork,
    IMediator mediator, 
    ILogger<UserCommandService> logger)
    : IUserCommandService
{
    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByEmailAsync(command.Email);

        if (!hashingService.VerifyPassword(command.Password, user.PasswordHash))
        {
            logger.LogWarning($"[IAM BC] Intento de inicio de sesión fallido para {command.Email}: contraseña inválida.");
            throw new Exception("Invalid username or password");
        }

        var token = tokenService.GenerateToken(user);

        user.RecordSignIn();

        logger.LogInformation(
            $"[IAM BC] Publicando {user.DomainEvents.Count} evento(s) de dominio después del inicio de sesión.");
        foreach (var domainEvent in user.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }

        user.ClearDomainEvents();

    logger.LogInformation($"[IAM BC] Usuario {command.Email} inició sesión exitosamente.");
        return (user, token);
    }

    /**
     * <summary>
     *     Handle sign-up command
     * </summary>
     * <param name="command">The sign-up command</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task<(User user, string token)> Handle(SignUpCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password) || string.IsNullOrWhiteSpace(command.PasswordConfirmation))
            throw new ArgumentException("Username and passwords cannot be empty");
        
        if (command.Password != command.PasswordConfirmation)
            throw new ArgumentException("Password and confirmation do not match.");
        
        if (await userRepository.ExistsByEmail(command.Email))
            throw new InvalidOperationException($"Email '{command.Email}' is already taken.");
        
        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = User.Create(Email.From(command.Email), hashedPassword);
        
        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in user.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        user.ClearDomainEvents();
        
        var token = tokenService.GenerateToken(user);

        return (user, token);
    }

    public async Task<bool> Handle(UpdatePasswordCommand command)
    {
        var user = await userRepository.FindByIdAsync(UserId.From(command.UserId));
        if (user == null) throw new ArgumentException("User not found.");

        var newHashedPassword = hashingService.HashPassword(command.NewPassword);
        user.UpdatePasswordHash(newHashedPassword);
        await unitOfWork.CompleteAsync();

        logger.LogInformation($"[IAM BC] Publicando {user.DomainEvents.Count} evento(s) de dominio después de actualizar contraseña.");
        foreach (var domainEvent in user.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        user.ClearDomainEvents();

        logger.LogInformation($"[IAM BC] Contraseña del usuario {command.UserId} actualizada.");
        return true;
    }
}