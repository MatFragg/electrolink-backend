namespace Hampcoders.Electrolink.API.Shared.Infrastructure;

public class AIProviderException : Exception
{
    public string ProviderName { get; }

    public AIProviderException(string providerName, string message)
        : base(message) => ProviderName = providerName;

    public AIProviderException(string providerName, string message, Exception inner)
        : base(message, inner) => ProviderName = providerName;
}
