using Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;
using Hampcoders.Electrolink.API.IAM.Domain.Model.Queries;
using Hampcoders.Electrolink.API.IAM.Domain.Services;

namespace Hampcoders.Electrolink.API.IAM.Interfaces.ACL.Services;

public class IamContextFacade(IUserCommandService userCommandService, IUserQueryService userQueryService) : IIamContextFacade
{
    public async Task<string> CreateUser(string username, string password)
    {
        var signUpCommand = new SignUpCommand(username, password);
        await userCommandService.Handle(signUpCommand);
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id.Value ?? string.Empty;
    }

    public async Task<string> FetchUserIdByUsername(string username)
    {
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id.Value ?? string.Empty;
    }

    public async Task<string> FetchUsernameByUserId(string userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        return result?.Username ?? string.Empty;
    }
    
    public async Task<bool> UserExistsAsync(string userId) {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var user = await userQueryService.Handle(getUserByIdQuery);
        return user != null;
    }
}