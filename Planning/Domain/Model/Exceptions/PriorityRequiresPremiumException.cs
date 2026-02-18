namespace Hampcoders.Electrolink.API.Planning.Domain.Model.Exceptions;

public class PriorityRequiresPremiumException : InvalidOperationException
{
    public PriorityRequiresPremiumException()
        : base("Priority requests require a Premium subscription plan") { }
}

