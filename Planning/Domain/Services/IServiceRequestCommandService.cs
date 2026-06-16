using Hampcoders.Electrolink.API.Planning.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

namespace Hampcoders.Electrolink.API.Planning.Domain.Services;

public interface IServiceRequestCommandService
{
    Task<RequestId?> Handle(InitiateServiceRequestCommand command);
    Task Handle(SelectPropertyForRequestCommand command);
    Task Handle(AddServiceDetailsCommand command);
    Task Handle(ConfirmServiceRequestCommand command);
    Task<bool> Handle(CancelServiceRequestCommand command);
    Task Handle(SelectServiceRecipeCommand command);
    Task Handle(MarkServiceRequestAsAssignedCommand command);
    Task Handle(ReactivateServiceRequestCommand command);
}

        