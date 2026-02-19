namespace Hampcoders.Electrolink.API.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    Task<string> CreateUser(string username, string password);
    Task<string> FetchUserIdByUsername(string username);
    Task<string> FetchUsernameByUserId(string userId);
    Task<bool> UserExistsAsync(string userId);
}