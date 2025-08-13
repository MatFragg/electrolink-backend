using Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;
using Hampcoders.Electrolink.API.IAM.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.IAM.Interfaces.REST.Transform;


public static class UpdatePasswordCommandFromResourceAssembler
{
    public static UpdatePasswordCommand ToCommandFromResource(int userId, UpdatePasswordResource resource)
    {
        return new UpdatePasswordCommand(userId, resource.CurrentPassword, resource.NewPassword);
    }
}
