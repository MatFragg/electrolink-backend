using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface ISubscriptionCommandService
{
    Task<Guid> Handle(CreateSubscriptionCommand command);
    Task Handle(CancelSubscriptionCommand command);
    Task Handle(PauseSubscriptionCommand command);
    Task Handle(ResumeSubscriptionCommand command);
    Task Handle(GrantPremiumAccessCommand command);
    Task Handle(ActivateBoostCommand command);
    Task Handle(VerifyCertificationCommand command);
}