namespace Hampcoders.Electrolink.API.Profiles.Domain.Model.Exceptions;

public class InvalidServiceAreaException : Exception
{
    public InvalidServiceAreaException(string message) : base(message) { }
}