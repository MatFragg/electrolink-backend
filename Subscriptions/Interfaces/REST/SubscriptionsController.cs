using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

/// <summary>
/// REST API controller for managing subscription plans.
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SubscriptionsController(ISubscriptionCommandService subscriptionCommandService, ISubscriptionQueryService queryService,  IConfiguration _cfg, IPlanQueryService planQueryService,ILogger<SubscriptionsController> logger ) : ControllerBase
{
    /// <summary>
    /// Creates a new subscription for a user.
    /// </summary>
    /// <param name="resource">The subscription creation resource.</param>
    /// <returns>The ID of the created subscription.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionResource resource)
    {
        // The validation for User existence and Technician existence/info retrieval
        // has been moved to the SubscriptionCommandService itself, as per CQRS principles
        // where commands encapsulate all necessary logic for their execution.
        var command = CreateSubscriptionCommandFromResourceAssembler.ToCommand(resource);
        try
        {
            var id = await subscriptionCommandService.Handle(command);
            var subscription = await queryService.Handle(new GetSubscriptionByIdQuery(id));
            if (subscription == null)
                return BadRequest(new { message = "Failed to retrieve created subscription." });

            var plan = await planQueryService.Handle(new GetPlanByIdQuery(subscription.PlanId.Value));
            if (plan == null)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Associated plan not found for created subscription." });

            var subscriptionResource = SubscriptionResourceFromEntityAssembler.ToResource(subscription, plan);
            return CreatedAtAction(nameof(GetById), new { id }, subscriptionResource);
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
    /// Gets a subscription by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the subscription.</param>
    /// <returns>The subscription resource if found, otherwise Not Found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var subscription = await queryService.Handle(new GetSubscriptionByIdQuery(id));
        if (subscription == null) return NotFound();

        var plan = await planQueryService.Handle(new GetPlanByIdQuery(subscription.PlanId.Value));
        if (plan == null)
        {
            // Log warning: Subscription found, but associated plan is missing.
            Console.WriteLine($"Warning: Subscription {id} found, but associated plan {subscription.PlanId.Value} not found.");
            // Return a default/basic resource if plan is missing, or NotFound if that's desired behavior
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Associated plan not found for this subscription." });
        }

        return Ok(SubscriptionResourceFromEntityAssembler.ToResource(subscription, plan));
    }

    /// <summary>
    /// Gets all subscriptions.
    /// </summary>
    /// <returns>A list of subscription resources.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SubscriptionResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var subscriptions = await queryService.Handle(new GetAllActiveSubscriptionsQuery()); // Or GetAllSubscriptionsQuery() if all are needed
        var resources = new List<SubscriptionResource>();

        foreach (var subscription in subscriptions)
        {
            var plan = await planQueryService.Handle(new GetPlanByIdQuery(subscription.PlanId.Value));
            if (plan != null)
            {
                resources.Add(SubscriptionResourceFromEntityAssembler.ToResource(subscription, plan));
            }
            else
            {
                Console.WriteLine($"Warning: Subscription {subscription.Id.Value} found, but associated plan {subscription.PlanId.Value} not found for GetAll.");
            }
        }
        return Ok(resources);
    }

    /// <summary>
    /// Cancels a subscription.
    /// </summary>
    /// <param name="id">The ID of the subscription to cancel.</param>
    /// <returns>No Content if successful, otherwise Not Found.</returns>
    [HttpPut("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        try
        {
            await subscriptionCommandService.Handle(new CancelSubscriptionCommand(id, DateTime.UtcNow.AddDays(30))); // Example: effective in 30 days
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Updates the status of a subscription (e.g., Pause, Resume).
    /// </summary>
    /// <param name="id">The ID of the subscription.</param>
    /// <param name="resource">The resource containing the new status.</param>
    /// <returns>No Content if successful, otherwise Not Found.</returns>
    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateSubscriptionStatusResource resource)
    {
        try
        {
            var command = UpdateSubscriptionStatusCommandFromResourceAssembler.ToCommand(id, resource);
            var updatedId = await subscriptionCommandService.Handle(command);
            return updatedId is null ? NotFound() : NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Change subscription plan internally (updates DB without Stripe interaction).
    /// Used for manual adjustments or migrations.
    /// </summary>
    [HttpPut("{subscriptionId:guid}/change-plan-internal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeSubscriptionPlanInternal(
        Guid subscriptionId,
        [FromBody] ChangeSubscriptionPlanInternalResource resource)
    {
        try
        {
            // Usa el assembler interno
            var command = ChangeSubscriptionPlanCommandFromResourceAssembler.ToCommand(
                subscriptionId, 
                resource);

            var result = await subscriptionCommandService.Handle(command);

            if (result == null)
            {
                return NotFound(new { message = $"Subscription {subscriptionId} not found" });
            }

            logger.LogInformation(
                "Subscription plan {SubscriptionId} changed internally to {NewPlanId}",
                subscriptionId,
                resource.NewPlanId);

            return Ok(new { 
                message = "Subscription plan updated successfully (internal)",
                subscriptionId = result
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Endpoint to create a Stripe Checkout Session for new subscriptions.
    /// This endpoint bypasses the standard CreateSubscriptionCommand for direct Stripe interaction,
    /// and then a webhook would handle the actual subscription creation in the system.
    /// </summary>
    /// <param name="req">The checkout session request.</param>
    /// <returns>A URL to the Stripe Checkout page.</returns>
    [HttpPost("checkout-session")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutSessionCommand req)
    {
        Stripe.StripeConfiguration.ApiKey = _cfg["Stripe:SecretKey"];

        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = req.PriceId.Value,
                    Quantity = 1
                }
            },
            SuccessUrl = _cfg["Stripe:SuccessUrl"],
            CancelUrl = _cfg["Stripe:CancelUrl"]
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return Ok(new { url = session.Url });
    }
    
    /// <summary>
    /// Gets a user's subscription eligibility and current benefits.
    /// This is an ACL endpoint to provide a simplified view for other Bounded Contexts or UIs.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A <see cref="SubscriptionEligibilityResource"/>.</returns>
    [HttpGet("users/{userId:int}/eligibility")]
    [ProducesResponseType(typeof(SubscriptionEligibilityResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserSubscriptionEligibility([FromRoute] int userId)
    {
        var subscription = await queryService.Handle(new GetSubscriptionByUserIdQuery(new UserId(   userId)));

        if (subscription == null)
        {
            return Ok(SubscriptionEligibilityAclAssembler.ToBasicResource(userId));
        }

        var plan = await planQueryService.Handle(new GetPlanByIdQuery(subscription.PlanId.Value));

        if (plan == null)
        {
            Console.WriteLine($"Warning: Active subscription found for user {userId} but associated plan {subscription.PlanId.Value} not found.");
            return Ok(SubscriptionEligibilityAclAssembler.ToBasicResource(userId));
        }

        var aclResource = SubscriptionEligibilityAclAssembler.ToResourceFromEntities(subscription, plan);
        return Ok(aclResource);
    }
}
