using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Queries;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;
using MediatR;
using Stripe;
using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Application.Internal.EventHandlers;

public class StripeEventHandler(IMediator mediator, IConfiguration configuration) 
    : IRequestHandler<ProcessStripeEventCommand>
{
    // Carga el secreto del webhook desde la configuración de forma segura.
    private readonly string _webhookSecret = configuration["Stripe:WebhookSecret"] 
                                            ?? throw new InvalidOperationException("Stripe Webhook Secret not configured.");

    public async Task<Unit> Handle(ProcessStripeEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Construye el evento de Stripe de forma segura, verificando la firma.
            var stripeEvent = EventUtility.ConstructEvent(
                request.StripeEventJson,
                request.StripeSignatureHeader,
                _webhookSecret
            );

            // Usa un 'switch' para manejar los diferentes tipos de eventos de Stripe.
            switch (stripeEvent.Type)
            {
                // Este evento se dispara cuando la sesión de checkout inicial se completa.
                case EventTypes.CheckoutSessionCompleted:
                    var session = stripeEvent.Data.Object as Session;
                    if (session?.Metadata == null || !session.Metadata.ContainsKey("local_subscription_id"))
                    {
                        Console.WriteLine("Checkout session completed event received but missing required metadata.");
                        return Unit.Value;
                    }

                    if (!Guid.TryParse(session.Metadata["local_subscription_id"], out var localSubscriptionIdGuid))
                    {
                        Console.WriteLine($"Invalid subscription ID format: {session.Metadata["local_subscription_id"]}");
                        return Unit.Value;
                    }    
                    if (session.PaymentIntentId == null) return Unit.Value;

                    var processInitialPaymentCommand = new ProcessPaymentCommand(
                        localSubscriptionIdGuid,
                        (decimal)session.AmountTotal.GetValueOrDefault() / 100,
                        session.Currency,
                        DateTime.UtcNow,
                        EPaymentStatus.Success,
                        session.PaymentIntentId,
                        "Pago de suscripción inicial a través de Checkout"  
                    );
                    
                    await mediator.Send(processInitialPaymentCommand, cancellationToken);
                    
                    Console.WriteLine($"Processed initial payment for subscription ID: {localSubscriptionIdGuid}");
                    break;

                // Este evento se dispara para los pagos recurrentes de la suscripción.
                case EventTypes.InvoicePaymentSucceeded:
                {
                    /*var invoice = stripeEvent.Data.Object as Stripe.Invoice;
                    if (invoice == null)
                    {
                        Console.WriteLine("El objeto no es una factura válida");
                        return Unit.Value;
                    }

// Imprimir todas las propiedades disponibles para depuración
                    foreach (var prop in invoice.GetType().GetProperties())
                    {
                        try
                        {
                            var value = prop.GetValue(invoice);
                            Console.WriteLine($"Propiedad: {prop.Name}, Valor: {value}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al leer propiedad {prop.Name}: {ex.Message}");
                        }
                    }*/
                    var invoice = stripeEvent.Data.Object as Stripe.Invoice;
                    if (invoice == null)
                    {
                        Console.WriteLine("El objeto no es una factura válida");
                        return Unit.Value;
                    }

                    var invoiceService = new InvoiceService();
                    invoice = invoiceService.Get(invoice.Id, new InvoiceGetOptions
                    {
                        Expand = new List<string> { "subscription" }
                    });

                    var subscriptionId = invoice.Lines?.Data?.FirstOrDefault()?.Subscription?.Id;

                    if (string.IsNullOrEmpty(subscriptionId))
                    {
                        Console.WriteLine("No subscription ID found in invoice.");
                        return Unit.Value;
                    }

                    // Ahora sí lo tienes garantizado
                    var getLocalIdQuery = new GetLocalSubscriptionIdQuery(subscriptionId);
                    var localSubscriptionIdResult = await mediator.Send(getLocalIdQuery, cancellationToken);

                    if (localSubscriptionIdResult is null)
                    {
                        Console.WriteLine($"No se encontró suscripción local para Stripe subscription: {subscriptionId}");
                        return Unit.Value;
                    }

                    var processRecurrentPaymentCommand = new ProcessPaymentCommand(
                        localSubscriptionIdResult.Value,
                        (decimal)invoice.AmountPaid / 100,
                        invoice.Currency,
                        DateTime.UtcNow,
                        EPaymentStatus.Success,
                        invoice.Id,
                        "Pago de suscripción recurrente"
                    );

                    await mediator.Send(processRecurrentPaymentCommand, cancellationToken);

                    Console.WriteLine($"Processed recurrent payment for subscription {subscriptionId}, invoice {invoice.Id}");
                    break;
                }

                
                // Si el evento no es uno de los anteriores, se ignora.
                default:
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }
        }
        catch (StripeException e)
        {
            Console.WriteLine($"Error de Stripe en webhook: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocurrió un error inesperado al procesar el webhook: {e.Message}");
            throw;
        }

        return Unit.Value;
    }
}