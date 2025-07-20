using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;
using Hamcoders.Electrolink.API.Monitoring.Domain.Model.Commands;

namespace Hamcoders.Electrolink.API.Monitoring.Domain.Services;

public interface IRatingCommandService
{
    Task Handle(AddRatingCommand command);
    Task Handle(DeleteRatingCommand command);
    Task Handle(UpdateRatingCommand command);
}

