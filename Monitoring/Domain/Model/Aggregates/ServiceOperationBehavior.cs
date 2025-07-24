using System.Linq;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Monitoring.Domain.Model.Entities;

namespace Hampcoders.Electrolink.API.Monitoring.Domain.Model.Aggregates;

public partial class ServiceOperation
{ public void Handle(UpdateServiceStatusCommand command)
    {
        if (command.RequestId != RequestId) return;
        ChangeStatus(command.NewStatus);
    }
}