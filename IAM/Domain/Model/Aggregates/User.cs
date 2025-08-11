using System.Text.Json.Serialization;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;

namespace Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;

/**
 * <summary>
 *     The user aggregate
 * </summary>
 * <remarks>
 *     This class is used to represent a user
 * </remarks>
 */
public partial class User(string username, string passwordHash)
{
    public User() : this(string.Empty, string.Empty)
    {
    }   

    public int Id { get; }
    public string Username { get; private set; } = username;
    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;

    /**
     * <summary>
     *     Update the username
     * </summary>
     * <param name="newUsername">The new username</param>
     * <returns>The updated user</returns>
     */
    public void UpdateUsername(string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername))
            throw new ArgumentException("Username cannot be empty or null.", nameof(newUsername));
        
        var oldUsername = Username;
        Username = newUsername;

        // Registra el evento de dominio
        _domainEvents.Add(new UsernameUpdatedEvent(Id, oldUsername, newUsername, DateTime.UtcNow));
    }

    /**
     * <summary>
     *     Update the password hash
     * </summary>
     * <param name="newPasswordHash">The new password hash</param>
     * <returns>The updated user</returns>
     */
    public void UpdatePasswordHash(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be empty or null.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;

        // Registra el evento de dominio
        _domainEvents.Add(new UserPasswordChangedEvent(Id, DateTime.UtcNow));
    }

    /**
     * <summary>
     *      Record Sign In event for the user.
     * </summary>
     */
    public void RecordSignIn()
    {
        _domainEvents.Add(new UserSignedInEvent(Id, DateTime.UtcNow));
    }

    /// <summary>
    /// Cleans the domain events list.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}