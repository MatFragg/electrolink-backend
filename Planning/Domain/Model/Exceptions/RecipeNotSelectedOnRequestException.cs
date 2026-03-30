namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class RecipeNotSelectedOnRequestException : DomainException
{
    public RecipeNotSelectedOnRequestException(string requestId)
        : base($"No recipe selected on request {requestId}.") { }

    public RecipeNotSelectedOnRequestException(Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects.RequestId requestId)
        : base($"No recipe selected on request {requestId.Value}.") { }
}