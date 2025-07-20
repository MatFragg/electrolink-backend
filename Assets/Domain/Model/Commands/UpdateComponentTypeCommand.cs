namespace Hampcoders.Electrolink.API.Assets.Domain.Model.Commands;

public record UpdateComponentTypeCommand(int Id, string Name, string? Description);
