using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

/// <summary>
/// <para>Interface for payment transaction command services.</para>
/// <para>Handles processing payment transactions, returning primitive IDs.</para>
/// </summary>
public interface IPaymentTransactionCommandService
{
    /// <summary>
    /// <para>Handles the command to process a payment.</para>
    /// </summary>
    /// <param name="command">The <see cref="ProcessPaymentCommand"/>.</param>
    /// <returns>The GUID of the created payment transaction.</returns>
    Task<Guid> Handle(ProcessPaymentCommand command);
}