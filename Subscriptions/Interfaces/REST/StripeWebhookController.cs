using Hampcoders.Electrolink.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/stripe/webhooks")]
public class StripeWebhookController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> HandleWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        
        if (!Request.Headers.TryGetValue("Stripe-Signature", out var signature))
        {
            return BadRequest("Missing Stripe-Signature header.");
        }

        try
        {
            var command = new ProcessStripeEventCommand(json, signature);
            await mediator.Send(command);
            
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while processing the Stripe webhook.",
                error = ex.Message
            });
        }
    }
}