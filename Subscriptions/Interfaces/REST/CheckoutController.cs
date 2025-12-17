using System.Security.Claims;
using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

/// <summary>
/// Controller to manage the Checkout flow and subscriptions with Stripe.
/// </summary>
[ApiController]
[Route("api/v1/subscriptions/checkout")]
[Authorize] 
[Produces("application/json")]
public class CheckoutController(
    ICheckoutCommandService checkoutCommandService, 
    IPaymentGatewayService paymentGateway, 
    ILogger<CheckoutController> logger
) : ControllerBase
{
    /// <summary>
    /// Creates a Stripe Checkout session to subscribe the user to a plan.
    /// </summary>
    [HttpPost("create-session")]
    [ProducesResponseType(typeof(CheckoutSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CheckoutSessionResource>> CreateCheckoutSession(
        [FromBody] CreateCheckoutSessionResource resource)
    {
        try
        {
            var userId = GetAuthenticatedUserId();

            // Create an Assembler for the command
            var command = new CreateCheckoutSessionCommand(
                new UserId(userId),
                new PlanId(resource.PlanId),
                resource.SuccessUrl,
                resource.CancelUrl,
                resource.TrialPeriodDays
            );

            var checkoutUrl = await checkoutCommandService.Handle(command);

            logger.LogInformation(
                "Checkout session created for User {UserId} - Plan {PlanId}",
                userId,
                resource.PlanId);

            return Ok(new CheckoutSessionResource(
                checkoutUrl,
                ExtractSessionIdFromUrl(checkoutUrl)
            ));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cancel the user's current subscription in Stripe.
    /// </summary>
    [HttpPost("{subscriptionId:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSubscription(
        Guid subscriptionId,
        [FromBody] CancelSubscriptionResource resource)
    {
        try
        {
            // Create and Assembler for the command
            var command = new CancelSubscriptionInGatewayCommand(
                new SubscriptionId(subscriptionId),
                resource.Immediately
            );

            await checkoutCommandService.Handle(command);

            logger.LogInformation(
                "Subscription {SubscriptionId} cancelled (Immediately: {Immediately})",
                subscriptionId,
                resource.Immediately);

            return Ok(new { 
                message = resource.Immediately 
                    ? "Subscription cancelled immediately" 
                    : "Subscription will be cancelled at the end of the billing period" 
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Change the plan of the user's current subscription via Stripe.
    /// </summary>
    [HttpPut("{subscriptionId:guid}/change-plan")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeSubscriptionPlan(
        Guid subscriptionId,
        [FromBody] ChangeSubscriptionPlanResource resource)
    {
        try
        {
            // Usa el comando específico de Stripe
            var command = new ChangeSubscriptionPlanInGatewayCommand(
                new SubscriptionId(subscriptionId),
                new PlanId(resource.NewPlanId),
                resource.ProrationBehavior
            );

            await checkoutCommandService.Handle(command);

            logger.LogInformation(
                "Subscription plan {SubscriptionId} changed to {NewPlanId}",
                subscriptionId,
                resource.NewPlanId);

            return Ok(new { 
                message = "Subscription plan updated successfully",
                subscriptionId,
                newPlanId = resource.NewPlanId
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a Stripe billing portal for the user to manage their subscription.
    /// </summary>
    [HttpPost("create-portal-session")]
    [ProducesResponseType(typeof(CheckoutSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CheckoutSessionResource>> CreateBillingPortalSession(
        [FromQuery] string returnUrl)
    {
        try
        {
            var userId = GetAuthenticatedUserId();

            // TODO: Obtener PaymentGatewayCustomerId del usuario desde el repositorio
            var stripeCustomerId = new Domain.Model.ValueObjects.PaymentGatewayCustomerId("cus_xxxxx");

            var portalUrl = await paymentGateway.CreateBillingPortalSessionAsync(
                stripeCustomerId,
                returnUrl
            );

            logger.LogInformation(
                "Billing portal created for User {UserId}",
                userId);

            return Ok(new CheckoutSessionResource(
                portalUrl,
                "portal_session"
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating billing portal");
            return BadRequest(new { message = "Failed to create billing portal session" });
        }
    }

    #region Helper Methods

    private int GetAuthenticatedUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        return userId;
    }

    private string ExtractSessionIdFromUrl(string checkoutUrl)
    {
        var uri = new Uri(checkoutUrl);
        var segments = uri.AbsolutePath.Split('/');
        return segments.Length > 0 ? segments[^1] : "unknown";
    }

    #endregion
}