using Hampcoders.Electrolink.API.Shared.Domain.Model.Events;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Events.Domain;
using Hampcoders.Electrolink.API.Subscriptions.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Subscriptions.Domain.Model.Aggregates;

/// <summary>
/// Represents a record of a payment transaction.
/// </summary>
public class PaymentTransaction
{
    /// <summary>
    /// Unique identifier for the payment transaction.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The ID of the subscription associated with this transaction.
    /// </summary>
    public SubscriptionId SubscriptionId { get; private set; }

    /// <summary>
    /// The amount of the transaction.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// The currency of the transaction.
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    /// The date and time when the transaction occurred.
    /// </summary>
    public DateTime TransactionDate { get; private set; }

    /// <summary>
    /// The status of the transaction (e.g., Success, Failed, Pending).
    /// </summary>
    public EPaymentStatus Status { get; private set; }

    /// <summary>
    /// The identifier from the payment gateway (e.g., Stripe Charge ID).
    /// </summary>
    public string GatewayTransactionId { get; private set; }

    /// <summary>
    /// Optional message or reason for the transaction status (e.g., error message).
    /// </summary>
    public string Message { get; private set; }
    
    
    /// <summary>
    ///  Domain events associated with this aggregate.
    /// </summary>
    private readonly List<IEvent> _domainEvents = new();
    
    /// <summary>
    /// Read-only collection of domain events.
    /// </summary>
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    
    /// <summary>
    /// Private constructor for ORM or deserialization.
    /// </summary>
    private PaymentTransaction() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentTransaction"/> class.
    /// </summary>
    /// <param name="subscriptionId">The ID of the associated subscription.</param>
    /// <param name="amount">The transaction amount.</param>
    /// <param name="currency">The transaction currency.</param>
    /// <param name="transactionDate">The date and time of the transaction.</param>
    /// <param name="status">The status of the transaction.</param>
    /// <param name="gatewayTransactionId">The payment gateway's transaction ID.</param>
    /// <param name="message">Optional message.</param>
    public PaymentTransaction(SubscriptionId subscriptionId, decimal amount, string currency, DateTime transactionDate, EPaymentStatus status, string gatewayTransactionId, string message = "")
    {
        Id = Guid.NewGuid();
        SubscriptionId = subscriptionId;
        Amount = amount;
        Currency = currency;
        TransactionDate = transactionDate;
        Status = status;
        GatewayTransactionId = gatewayTransactionId;
        Message = message;
        
        _domainEvents.Add(new PaymentProcessedEvent(
            Id,
            subscriptionId.Value,
            amount,
            status,
            gatewayTransactionId,
            DateTime.UtcNow));
    }

    /// <summary>
    /// Updates the status and message of the payment transaction.
    /// </summary>
    /// <param name="newStatus">The new payment status.</param>
    /// <param name="newMessage">An updated message.</param>
    public void UpdateStatus(EPaymentStatus newStatus, string newMessage = "")
    {
        var oldStatus = Status;
        if (oldStatus == newStatus && Message == newMessage) return;
        Status = newStatus;
        Message = newMessage;
        
        _domainEvents.Add(new PaymentStatusUpdatedEvent(
            Id,
            SubscriptionId.Value,
            oldStatus,
            newStatus,
            DateTime.UtcNow));
    }
    
    /// <summary>
    /// Clears all domain events from the transaction's event list.
    ///</summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }   
}