namespace Hampcoders.Electrolink.API.Shared.Infrastructure;

public class PaymentProviderException : Exception
{
    public string ProviderName { get; }
    public string? ProviderErrorCode { get; }

    public PaymentProviderException(string providerName, string message)
        : base(message) => ProviderName = providerName;

    public PaymentProviderException(string providerName, string message, Exception inner, string? providerErrorCode = null)
        : base(message, inner)
    {
        ProviderName = providerName;
        ProviderErrorCode = providerErrorCode;
    }
}
