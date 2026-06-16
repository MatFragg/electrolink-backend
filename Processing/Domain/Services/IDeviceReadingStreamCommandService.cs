using Hampcoders.Electrolink.API.Processing.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Processing.Domain.Services;

public interface IDeviceReadingStreamCommandService
{
    Task<bool> Handle(IngestDeviceReadingCommand command);
    Task Handle(ReportEdgeAnomalyCommand command);
    Task Handle(ReplayBufferedReadingsCommand command);
    Task Handle(MarkDeviceAsDisconnectedCommand command);
    Task Handle(UpdateCustomThresholdsCommand command);
    Task Handle(PauseDeviceStreamCommand command);
    Task Handle(ResumeDeviceStreamCommand command);
}
