using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Shared.Interfaces.REST;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;
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
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryService subscriptionQueryService,
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
            var userId = this.GetAuthenticatedUserIdOrThrow();

            // Create an Assembler for the command
            var createCheckoutSessionCommand = CreateCheckoutSessionCommandFromResourceAssembler
                .ToCommandFromResource(userId.ToString(), resource);
            
            
            var checkoutUrl = await subscriptionCommandService.Handle(createCheckoutSessionCommand);

            logger.LogInformation(
                "Checkout session created for User {UserId} - Price {PriceId}",
                userId,
                resource.PriceId);

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
            var command = new CancelSubscriptionCommand(
                new SubscriptionId(subscriptionId),
                resource.Immediately
            );

            await subscriptionCommandService.Handle(command);

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
                new PaymentGatewayPriceId(resource.NewPriceId),
                resource.ProrationBehavior
            );

            await subscriptionCommandService.Handle(command);

            logger.LogInformation(
                "Subscription price {SubscriptionId} changed to {NewPriceId}",
                subscriptionId,
                resource.NewPriceId);

            return Ok(new {
                message = "Subscription plan updated successfully",
                subscriptionId,
                newPriceId = resource.NewPriceId
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
    [HttpPost("billing-portal")]
    [ProducesResponseType(typeof(CheckoutSessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CheckoutSessionResource>> CreateBillingPortalSession(
        [FromBody] CreateBillingPortalSessionResource resource)
    {
        try
        {
            var userId = this.GetAuthenticatedUserIdOrThrow();
        
            var checkOutCommand = CreateBillingPortalSessionCommandFromResourceAssembler
                .ToCommand(userId, resource);
            
            var portalUrl = await subscriptionCommandService.Handle(checkOutCommand);
        
            return Ok(new CheckoutSessionResource(portalUrl));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    #region Helper Methods

    private string ExtractSessionIdFromUrl(string checkoutUrl)
    {
        var uri = new Uri(checkoutUrl);
        var segments = uri.AbsolutePath.Split('/');
        return segments.Length > 0 ? segments[^1] : "unknown";
    }

    #endregion
}