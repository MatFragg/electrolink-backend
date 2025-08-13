using Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;
using Hampcoders.Electrolink.API.IAM.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.IAM.Interfaces.REST.Transform;

public static class UpdateUsernameCommandFromResourceAssembler
{
    public static UpdateUsernameCommand ToCommandFromResource(int userId, UpdateUsernameResource resource)
    {
        return new UpdateUsernameCommand(userId, resource.Username);
    }
}