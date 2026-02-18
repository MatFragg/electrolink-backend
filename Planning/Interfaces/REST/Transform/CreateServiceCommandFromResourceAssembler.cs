using Hampcoders.Electrolink.API.Planning.Domain.Model.Commands;
using Hampcoders.Electrolink.API.Planning.Interfaces.REST.Resources;


namespace Hampcoders.Electrolink.API.Planning.Interfaces.REST.Transform;

public static class CreateServiceCommandFromResourceAssembler
{
    public static CreateServiceCommand ToCommandFromResource(CreateServiceResource r) =>
        new CreateServiceCommand(
            r.Name,
            r.Description,
            r.BasePrice,
            r.EstimatedTime,
            r.Category,
            r.IsVisible,
            r.CreatedBy
        );
}
