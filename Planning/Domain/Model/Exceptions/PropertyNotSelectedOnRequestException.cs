namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class PropertyNotSelectedOnRequestException : DomainException
{
    public PropertyNotSelectedOnRequestException(string requestId)
        : base($"No property selected on request {requestId}.") { }

    public PropertyNotSelectedOnRequestException(Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects.RequestId requestId)
        : base($"No property selected on request {requestId.Value}.") { }
}