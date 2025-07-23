using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Services;

public interface IReportCommandService
{
    Task Handle(AddReportCommand command);
    Task Handle(AddReportPhotoCommand command);
    Task Handle(DeleteReportCommand command);
}