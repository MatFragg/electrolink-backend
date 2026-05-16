namespace Hampcoders.Electrolink.API.Shared.Infrastructure;

public class FileStorageProviderException : Exception
{
    public string ProviderName { get; }

    public FileStorageProviderException(string providerName, string message)
        : base(message) => ProviderName = providerName;

    public FileStorageProviderException(string providerName, string message, Exception inner)
        : base(message, inner) => ProviderName = providerName;
}
