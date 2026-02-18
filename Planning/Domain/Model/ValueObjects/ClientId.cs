namespace Hampcoders.Electrolink.API.Planning.Domain.Model.ValueObjects;

public record ClientId(int Id)
{
    public override string ToString() => Id.ToString();
}