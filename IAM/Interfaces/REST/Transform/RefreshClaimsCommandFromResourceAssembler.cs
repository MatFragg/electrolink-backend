using Hampcoders.Electrolink.API.IAM.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.IAM.Interfaces.REST.Transform;

public static class RefreshClaimsCommandFromResourceAssembler
{
    public static RefreshClaimsCommand ToCommandFromResource(string userId)
        => new(UserId.From(userId));
}