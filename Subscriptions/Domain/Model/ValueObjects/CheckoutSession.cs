namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

public record CheckoutSession(
    CheckoutSessionId SessionId,        
    Uri CheckoutUrl,                    
    DateTime ExpiresAt,                 
    CheckoutSessionStatus Status,        
    decimal Amount,                       
    string Currency                    
);