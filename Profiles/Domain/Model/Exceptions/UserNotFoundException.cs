namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

[Serializable]
public sealed class UserNotFoundException : Exception
{
    public string UserId { get; }

    public UserNotFoundException(string userId)
        : base($"User with id '{userId}' not found.")
    {
        UserId = userId;
    }

    public UserNotFoundException(string userId, Exception innerException)
        : base($"User with id '{userId}' not found.", innerException)
    {
        UserId = userId;
    }
}