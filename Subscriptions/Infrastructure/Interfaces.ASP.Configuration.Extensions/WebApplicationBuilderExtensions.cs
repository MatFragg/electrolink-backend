using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.CommandServices;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.OutboundServices;
using Hampcoders.Electrolink.API.Subscriptions.Application.Internal.QueryServices;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Repository;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Services;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.BackgroundServices;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.PaymentGateway.Stripe;
using Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

namespace Hampcoders.Electrolink.API.Subscriptions.Infrastructure.Interfaces.ASP.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddSubscriptionServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IStripeService, StripeService>();

        builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        builder.Services.AddScoped<IPaymentRecordRepository, PaymentRecordRepository>();

        builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
        builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();
        builder.Services.AddScoped<ExternalIamService>();
        builder.Services.AddScoped<ExternalProfileService>();

        builder.Services.AddHostedService<GracePeriodExpirationJob>();
        builder.Services.AddHostedService<MonthlyCounterResetJob>();
    }
}