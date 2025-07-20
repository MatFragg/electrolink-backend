namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record ComponentId(Guid Id){
    public static ComponentId NewId() => new(Guid.NewGuid());
}