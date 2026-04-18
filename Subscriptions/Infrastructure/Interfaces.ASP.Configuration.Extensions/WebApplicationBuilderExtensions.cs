using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe.Webhooks;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddSubscriptionServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SessionService>();
        builder.Services.AddSingleton<StripeClientFactory>();
        builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
        builder.Services.AddSingleton<StripeSettings>(sp => sp.GetRequiredService<IOptions<StripeSettings>>().Value);
        builder.Services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();
        builder.Services.AddScoped<IWebhookEventRepository, WebhookEventRepository>();
        builder.Services.AddScoped<StripeWebhookValidator>();
        builder.Services.AddScoped<StripeWebhookEventProcessor>();
        builder.Services.AddScoped<StripeWebhookCommandService>();

       
        builder.Services.AddScoped<IPlanRepository, PlanRepository>();
        builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
        builder.Services.AddScoped<IPlanCommandService, PlanCommandService>();
        builder.Services.AddScoped<IPlanQueryService, PlanQueryService>();
        builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
        builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();
        builder.Services.AddScoped<IPaymentTransactionQueryService,PaymentTransactionQueryService>();
        builder.Services.AddScoped<IPaymentTransactionCommandService,PaymentTransactionCommandService>();
        builder.Services.AddScoped<ExternalIamService>();
        builder.Services.AddScoped<ExternalProfileService>();
        
        builder.Services.AddScoped<StripeEventMapper>();
        
    }
}