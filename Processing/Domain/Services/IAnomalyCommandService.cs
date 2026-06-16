using Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Processing.Domain.Services;

public interface IAnomalyCommandService
{
    Task Handle(EvaluateReadingForAnomaliesCommand command);
    Task Handle(ResolveAnomalyCommand command);
    Task Handle(AcknowledgeAnomalyCommand command);
    Task Handle(ForceResolveAnomalyCommand command);
    Task Handle(AutoIssueRelayCommandCommand command);
}
