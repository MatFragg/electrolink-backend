using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IServiceRequestCommandService
{
    Task<ServiceRequest?> Handle(InitiateServiceRequestCommand command);
    Task<ServiceRequest?> Handle(SelectPropertyForRequestCommand command);
    Task<ServiceRequest?> Handle(AddServiceDetailsCommand command);
    Task<ServiceRequest?> Handle(ConfirmServiceRequestCommand command);
    Task<bool> Handle(CancelServiceRequestCommand command);
}

