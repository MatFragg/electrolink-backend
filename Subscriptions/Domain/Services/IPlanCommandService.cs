using Hamcoders.Electrolink.API.Subscriptions.Domain.Model.Commands;

namespace Hamcoders.Electrolink.API.Subscriptions.Domain.Services;

public interface IPlanCommandService
{
    Task<Guid> Handle(CreatePlanCommand command);
    Task Handle(UpdatePlanCommand command);
    Task Handle(DeletePlanCommand command);
    Task Handle(MarkPlanAsDefaultCommand command);
}