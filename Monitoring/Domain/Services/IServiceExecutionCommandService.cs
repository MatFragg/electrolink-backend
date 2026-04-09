using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Services;

/// <summary>
/// Command service interface for handling service execution commands.
/// </summary>
public interface IServiceExecutionCommandService
{
    Task<ServiceExecution> Handle(CreateServiceExecutionCommand command);
    Task<ServiceExecution> Handle(StartServiceExecutionCommand command);
    Task<ServiceExecution> Handle(UploadWorkPhotoCommand command);
    Task<ServiceExecution> Handle(RecordComponentsUsedCommand command);
    Task<ServiceExecution> Handle(UpdateTechnicalReportCommand command);
    Task<ServiceExecution> Handle(CompleteServiceExecutionCommand command);
    Task<ServiceExecution> Handle(CancelServiceExecutionCommand command);
    Task<ServiceExecution> Handle(ExtendServiceWaitTimeCommand command);
    Task<ServiceExecution> Handle(SubmitHomeownerReviewCommand command);
    Task<ServiceExecution> Handle(SubmitTechnicianReviewCommand command);
}

