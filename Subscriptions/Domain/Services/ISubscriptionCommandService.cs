using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Services;

/// <summary>
/// <para>Interface for subscription command services.</para>
/// <para>Handles creating, updating, and deleting subscriptions, returning primitive IDs or void.</para>
/// </summary>
public interface ISubscriptionCommandService
{
    /// <summary>
    /// <para>Handles the command to create a new subscription.</para>
    /// </summary>
    /// <param name="command">The <see cref="CreateSubscriptionCommand"/>.</param>
    /// <returns>The GUID of the created subscription.</returns>
    Task<Guid> Handle(CreateSubscriptionCommand command);

    /// <summary>
    /// <para>Handles the command to update the status of a subscription.</para>
    /// </summary>
    /// <param name="command">The <see cref="UpdateSubscriptionStatusCommand"/>.</param>
    /// <returns>The GUID of the updated subscription, or null if not found.</returns>
    Task<Guid?> Handle(UpdateSubscriptionStatusCommand command);

    /// <summary>
    /// <para>Handles the command to cancel a subscription.</para>
    /// </summary>
    /// <param name="command">The <see cref="CancelSubscriptionCommand"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Handle(CancelSubscriptionCommand command); // Void return

    /// <summary>
    /// <para>Handles the command to activate a trial for a subscription.</para>
    /// </summary>
    /// <param name="command">The <see cref="ActivateTrialCommand"/>.</param>
    /// <returns>The GUID of the updated subscription, or null if not found.</returns>
    Task<Guid?> Handle(ActivateTrialCommand command);

    /// <summary>
    /// <para>Handles the command to increment the usage counter for a subscription.</para>
    /// </summary>
    /// <param name="command">The <see cref="IncrementSubscriptionUsageCommand"/>.</param>
    /// <returns>The GUID of the updated subscription, or null if not found.</returns>
    Task<Guid?> Handle(IncrementSubscriptionUsageCommand command);

    /// <summary>
    /// <para>Handles the command to reset the usage counter for a subscription.</para>
    /// </summary>
    /// <param name="command">The <see cref="ResetSubscriptionUsageCommand"/>.</param>
    /// <returns>The GUID of the updated subscription, or null if not found.</returns>
    Task<Guid?> Handle(ResetSubscriptionUsageCommand command);

    /// <summary>
    /// <para>Handles the command to apply a discount to a subscription.</para>
    /// </summary>
    /// <param name="command">The <see cref="ApplyDiscountCommand"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Handle(ApplyDiscountCommand command); // Void return

    /// <summary>
    /// <para>Handles the command to change the plan of a subscription.</para>
    /// </summary>
    /// <param name="command">The <see cref="ChangeSubscriptionPlanCommand"/>.</param>
    /// <returns>The GUID of the updated subscription, or null if not found.</returns>
    Task<Guid?> Handle(ChangeSubscriptionPlanCommand command);

    /// <summary>
    /// <p> Handles the command to create a Checkout session in Stripe.</p>
    /// </summary>
    /// <param name="command">The <see cref="CreateCheckoutSessionCommand"/>.</param>
    /// <returns>The session ID of the created Checkout session.</returns>  
    Task<string> Handle(CreateCheckoutSessionCommand command);

    /// <summary>
    /// <p> Handles the command to process a payment.</p>
    /// </summary>
    /// <param name="command">The <see cref="ProcessPaymentCommand"/>.</param>
    /// <returns>The payment ID of the processed payment.</returns>
    Task<Guid> Handle(ProcessPaymentCommand command);

    /// <summary>
    /// <p> Handles the command to cancel a subscription in the payment gateway.</p>
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task Handle(CancelSubscriptionInGatewayCommand command);

    /// <summary>
    /// <p> Handles the command to change a subscription plan in the payment gateway.</p>
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task Handle(ChangeSubscriptionPlanInGatewayCommand command);

    /// <summary>
    /// <p> Handles the command to sync a subscription from the payment gateway.</p>
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task Handle(SyncSubscriptionFromGatewayCommand command);
}