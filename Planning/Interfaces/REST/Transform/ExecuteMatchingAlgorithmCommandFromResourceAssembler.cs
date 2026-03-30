using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class ExecuteMatchingAlgorithmCommandFromResourceAssembler
{
    public static ExecuteMatchingAlgorithmCommand ToCommand(
        ExecuteMatchingAlgorithmResource resource)
        => new(RequestId.From(resource.RequestId));
}