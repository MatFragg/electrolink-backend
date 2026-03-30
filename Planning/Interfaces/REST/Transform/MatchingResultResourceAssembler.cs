using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;

namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class MatchingResultResourceAssembler
{
    public static MatchingResultResource ToResource(
        string  requestId,
        bool    assigned,
        string? failureReason = null)
        => new(requestId, assigned, failureReason);
}