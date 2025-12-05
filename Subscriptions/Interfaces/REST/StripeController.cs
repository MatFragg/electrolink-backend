using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Hampcoders.Electrolink.API.Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/checkout")]
public class StripeController(IStripeService stripeService) : ControllerBase
{
    /// <summary>
    /// Creates a Stripe Checkout Session to handle a new subscription payment.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionResource resource)
    {
        // Llama al servicio de infraestructura para crear la sesión de Stripe
        var session = await stripeService.CreateSubscriptionCheckoutSession(
            resource.PriceId,
            resource.SuccessUrl,
            resource.CancelUrl
        );

        // Devuelve la URL de la sesión de Stripe al cliente
        return Ok(new { url = session.Url });
    }
}