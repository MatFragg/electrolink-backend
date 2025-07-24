using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SubscriptionsController(ISubscriptionCommandService commandService, ISubscriptionQueryService queryService,  IConfiguration _cfg) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionResource resource)
    {
        var command = CreateSubscriptionCommandFromResourceAssembler.ToCommand(resource);
        var id = await commandService.Handle(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var subscription = await queryService.Handle(new GetSubscriptionByIdQuery(id));
        return subscription is null
            ? NotFound()
            : Ok(SubscriptionResourceFromEntityAssembler.ToResource(subscription));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var subscriptions = await queryService.Handle(new GetAllSubscriptionsQuery());
        var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResource);
        return Ok(resources);
    }

    [HttpPut("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        await commandService.Handle(new CancelSubscriptionCommand(id));
        return NoContent();
    }

    [HttpPut("{id:guid}/resume")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Resume([FromRoute] Guid id)
    {
        await commandService.Handle(new ResumeSubscriptionCommand(id));
        return NoContent();
    }

    [HttpPut("{id:guid}/pause")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Pause([FromRoute] Guid id)
    {
        await commandService.Handle(new PauseSubscriptionCommand(id));
        return NoContent();
    }

    [HttpPut("{id:guid}/grant-premium")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GrantPremium([FromRoute] Guid id, [FromQuery] DateTime until)
    {
        await commandService.Handle(new GrantPremiumAccessCommand(id, until));
        return NoContent();
    }

    [HttpPut("{id:guid}/verify-certification")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> VerifyCertification([FromRoute] Guid id, [FromQuery] DateTime now)
    {
        await commandService.Handle(new VerifyCertificationCommand(id, now));
        return NoContent();
    }

    [HttpPut("{id:guid}/activate-boost")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ActivateBoost([FromRoute] Guid id, [FromQuery] DateTime now)
    {
        await commandService.Handle(new ActivateBoostCommand(id, now));
        return NoContent();
    }
    
    [HttpPost("session")]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutSessionRequest req)
    {
        Stripe.StripeConfiguration.ApiKey = _cfg["Stripe:SecretKey"];

        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = "price_1RjXmlPOfORd3gqgBlMsuRlT",
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
}
