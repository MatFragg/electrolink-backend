using System.Security.Claims;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/subscriptions")]
public class SubscriptionsController(
    ISubscriptionCommandService commandService,
    ISubscriptionQueryService   queryService) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMySubscription()
    {
        var userId       = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var subscription = await queryService.Handle(new GetMySubscriptionQuery(userId));
        return Ok(MySubscriptionResourceFromEntityAssembler.ToResource(subscription));
    }

    [HttpGet("me/eligibility")]
    [Authorize]
    public async Task<IActionResult> GetRequestEligibility()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await queryService.Handle(new GetRequestEligibilityQuery(userId));
        return Ok(RequestEligibilityResourceFromEntityAssembler.ToResource(result));
    }

    [HttpGet("me/payment-history")]
    [Authorize]
    public async Task<IActionResult> GetPaymentHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var records = await queryService.Handle(new GetPaymentHistoryQuery(userId, page, pageSize));
        return Ok(PaymentHistoryResourceFromEntityAssembler.ToResource(records, page, pageSize));
    }

    [HttpGet("me/status-alert")]
    [Authorize]
    public async Task<IActionResult> GetStatusAlert()
    {
        var userId       = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var subscription = await queryService.Handle(new GetSubscriptionStatusAlertQuery(userId));

        if (subscription is null) return NoContent();

        return Ok(SubscriptionStatusAlertResourceFromEntityAssembler.ToResource(subscription, null));
    }

    [HttpPost("checkout")]
    [Authorize]
    public async Task<IActionResult> InitiateCheckout([FromBody] InitiateCheckoutResource resource)
    {
        var userId     = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command    = InitiateCheckoutCommandFromResourceAssembler.ToCommand(userId, resource);
        var result = await commandService.Handle(command);
        return Ok(new CheckoutUrlResource(result.CheckoutUrl, result.SessionId));
    }

    [HttpPost("me/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelSubscription([FromBody] CancelSubscriptionResource resource)
    {
        var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = CancelSubscriptionCommandFromResourceAssembler.ToCommand(userId, resource);
        var subscription = await commandService.Handle(command);
        return Ok(MySubscriptionResourceFromEntityAssembler.ToResource(subscription));
    }

    [HttpPost("me/portal")]
    [Authorize]
    public async Task<IActionResult> OpenCustomerPortal([FromBody] OpenCustomerPortalResource resource)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new OpenCustomerPortalCommand(userId, resource.ReturnUrl);
        var result = await commandService.Handle(command);
        return Ok(new CustomerPortalUrlResource(result.PortalUrl));
    }
}
