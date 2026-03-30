using Hampcoders.Electrolink.API.IAM.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.IAM.Interfaces.REST.Transform;

public static class RefreshedTokenResourceFromTokenAssembler
{
    public static RefreshedTokenResource ToResourceFromToken(string token)
        => new(token);
}