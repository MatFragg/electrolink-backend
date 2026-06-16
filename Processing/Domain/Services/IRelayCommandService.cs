using Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Processing.Domain.Services;

public interface IRelayCommandService
{
    Task Handle(IssueRelayCommandCommand command);
    Task Handle(AcknowledgeRelayExecutionCommand command);
}
