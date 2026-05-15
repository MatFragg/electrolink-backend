using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionCommandService _commandService;
    private readonly ISubscriptionQueryService   _queryService;
    private readonly IStripeService              _stripeService;

    public SubscriptionsController(
        ISubscriptionCommandService commandService,
        ISubscriptionQueryService   queryService,
        IStripeService              stripeService)
    {
        _commandService = commandService;
        _queryService   = queryService;
        _stripeService  = stripeService;
    }

    // GET /api/v1/subscriptions/me
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMySubscription()
    {
        var userId       = HttpContext.Items["UserId"]!.ToString()!;
        var subscription = await _queryService.Handle(new GetMySubscriptionQuery(userId));
        return Ok(MySubscriptionResourceFromEntityAssembler.ToResource(subscription));
    }

    // GET /api/v1/subscriptions/me/eligibility
    [HttpGet("me/eligibility")]
    [Authorize]
    public async Task<IActionResult> GetRequestEligibility()
    {
        var userId = HttpContext.Items["UserId"]!.ToString()!;
        var result = await _queryService.Handle(new GetRequestEligibilityQuery(userId));
        return Ok(RequestEligibilityResourceFromEntityAssembler.ToResource(result));
    }

    // GET /api/v1/subscriptions/me/payment-history
    [HttpGet("me/payment-history")]
    [Authorize]
    public async Task<IActionResult> GetPaymentHistory()
    {
        var userId  = HttpContext.Items["UserId"]!.ToString()!;
        var records = await _queryService.Handle(new GetPaymentHistoryQuery(userId));
        return Ok(PaymentHistoryResourceFromEntityAssembler.ToResource(records));
    }

    // GET /api/v1/subscriptions/me/status-alert
    [HttpGet("me/status-alert")]
    [Authorize]
    public async Task<IActionResult> GetStatusAlert()
    {
        var userId       = HttpContext.Items["UserId"]!.ToString()!;
        var subscription = await _queryService.Handle(new GetSubscriptionStatusAlertQuery(userId));

        if (subscription is null) return NoContent();

        string? portalUrl = null;
        if (subscription.StripeCustomerId is not null)
            portalUrl = await _stripeService.CreateCustomerPortalSessionAsync(
                subscription.StripeCustomerId.Value,
                $"{Request.Scheme}://{Request.Host}/settings/subscription");

        return Ok(SubscriptionStatusAlertResourceFromEntityAssembler.ToResource(subscription, portalUrl));
    }

    // POST /api/v1/subscriptions/checkout
    [HttpPost("checkout")]
    [Authorize]
    public async Task<IActionResult> InitiateCheckout([FromBody] InitiateCheckoutResource resource)
    {
        var userId     = HttpContext.Items["UserId"]!.ToString()!;
        var command    = InitiateCheckoutCommandFromResourceAssembler.ToCommand(userId, resource);
        var checkoutUrl = await _commandService.Handle(command);
        return Ok(new CheckoutUrlResource(checkoutUrl));
    }

    // DELETE /api/v1/subscriptions/me
    [HttpDelete("me")]
    [Authorize]
    public async Task<IActionResult> CancelSubscription([FromBody] CancelSubscriptionResource resource)
    {
        var userId  = HttpContext.Items["UserId"]!.ToString()!;
        var command = CancelSubscriptionCommandFromResourceAssembler.ToCommand(userId, resource);
        var subscription = await _commandService.Handle(command);
        return Ok(MySubscriptionResourceFromEntityAssembler.ToResource(subscription));
    }

    // POST /api/v1/subscriptions/me/portal
    [HttpPost("me/portal")]
    [Authorize]
    public async Task<IActionResult> OpenCustomerPortal([FromBody] OpenCustomerPortalResource resource)
    {
        var userId       = HttpContext.Items["UserId"]!.ToString()!;
        var subscription = await _queryService.Handle(new GetMySubscriptionQuery(userId));

        if (subscription.StripeCustomerId is null)
            return BadRequest("No Stripe customer associated with this subscription.");

        var portalUrl = await _stripeService.CreateCustomerPortalSessionAsync(
            subscription.StripeCustomerId.Value,
            resource.ReturnUrl);

        return Ok(new CustomerPortalUrlResource(portalUrl));
    }
}
