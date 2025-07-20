namespace Hampcoders.Electrolink.API.Assets.Domain.Model.ValueObjects;

public record PropertyId
{
    public Guid Id { get; }

    public PropertyId(Guid value)
    {
        // AÑADIDO: Validación para prevenir un estado inválido.
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Property ID cannot be empty.", nameof(value));
        }
        Id = value;
    }

    public static PropertyId NewId() => new(Guid.NewGuid());
}