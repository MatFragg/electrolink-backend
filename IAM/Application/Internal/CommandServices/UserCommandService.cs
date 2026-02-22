using Hampcoders.Electrolink.API.IAM.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.IAM.Domain.Repositories;
using Hampcoders.Electrolink.API.IAM.Domain.Services;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Repositories;
using Hampcoders.Electrolink.API.Shared.Domain.Services;
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
    IIntegrationEventPublisher integrationEventPublisher,
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
        var user = await userRepository.FindByEmailAsync(command.Username);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        var token = tokenService.GenerateToken(user);

        user.RecordSignIn();

        logger.LogInformation(
            $"[IAM BC] Publicando {user.DomainEvents.Count} evento(s) de dominio después del inicio de sesión.");
        foreach (var domainEvent in user.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }

        user.ClearDomainEvents();

    logger.LogInformation($"[IAM BC] Usuario {command.Username} inició sesión exitosamente.");
        return (user, token);
    }

    /**
     * <summary>
     *     Handle sign-up command
     * </summary>
     * <param name="command">The sign-up command</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task Handle(SignUpCommand command)
    {
        if (command.Password != command.PasswordConfirmation)
            throw new ArgumentException("Password and confirmation do not match.");
        
        if (userRepository.ExistsByEmail(command.Email))
            throw new InvalidOperationException($"Email '{command.Email}' is already taken.");
        
        var user = User.Create(Email.Create(command.Email), command.Password);
        
        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();
        
        foreach (var domainEvent in user.DomainEvents)
            await mediator.Publish(domainEvent, CancellationToken.None);
        user.ClearDomainEvents();
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