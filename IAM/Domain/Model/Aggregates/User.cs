using Hampcoders.Electrolink.API.IAM.Domain.Model.Events;
using Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;

/**
 * <summary>
 *     The user aggregate
 * </summary>
 * <remarks>
 *     This class is used to represent a user
 * </remarks>
 */
public class User : BaseAggregateRoot
{
    public UserId Id { get; private set; }
    public Email Email { get; private set; }
    public HashedPassword PasswordHash { get; private set; }
    private User() { }
    
    public static User Create(Email email, HashedPassword passwordHash)
    {
        var user = new User
        {
            Id = UserId.NewUserId(),
            Email = email,
            PasswordHash = passwordHash
        };

        user.RaiseDomainEvent(new UserRegisteredEvent(user.Id.Value, user.Email.Value, DateTime.UtcNow));

        return user;
    }

    /**
     * <summary>
     *     Update the password hash
     * </summary>
     * <param name="newPasswordHash">The new password hash</param>
     * <returns>The updated user</returns>
     */
    public void UpdatePasswordHash(HashedPassword newPasswordHash)
    {
        PasswordHash = newPasswordHash;

        RaiseDomainEvent(new UserPasswordChangedEvent(Id.Value, DateTime.UtcNow));
    }

    /**
     * <summary>
     *      Record Sign In event for the user.
     * </summary>
     */
    public void RecordSignIn()
    {
        RaiseDomainEvent(new UserSignedInEvent(Id.Value, DateTime.UtcNow));
    }
}