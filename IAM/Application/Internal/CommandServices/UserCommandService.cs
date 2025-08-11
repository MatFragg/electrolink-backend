using Hampcoders.Electrolink.API.IAM.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;
using Hampcoders.Electrolink.API.IAM.Domain.Repositories;
using Hampcoders.Electrolink.API.IAM.Domain.Services;
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
        var user = await userRepository.FindByUsernameAsync(command.Username);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        var token = tokenService.GenerateToken(user);

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
        try
        {
            if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Password))
                throw new ArgumentException("Username and password cannot be empty");
                
            if (userRepository.ExistsByUsername(command.Username))
                throw new Exception($"Username {command.Username} is already taken");

            var hashedPassword = hashingService.HashPassword(command.Password);
            var user = new User(command.Username, hashedPassword);
        
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating user: {e.Message}");
        }
    }
    
    public async Task<bool> Handle(UpdateUsernameCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId); 
        if (user == null) throw new ArgumentException("User not found.");

        user.UpdateUsername(command.NewUsername); 
        await unitOfWork.CompleteAsync();

        logger.LogInformation($"[IAM BC] Publicando {user.DomainEvents.Count} evento(s) de dominio después de actualizar username.");
        foreach (var domainEvent in user.DomainEvents)
        {
            await mediator.Publish(domainEvent, CancellationToken.None);
        }
        user.ClearDomainEvents();

        logger.LogInformation($"[IAM BC] Username del usuario {command.UserId} actualizado a {command.NewUsername}.");
        return true;
    }

    public async Task<bool> Handle(UpdatePasswordCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId); 
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