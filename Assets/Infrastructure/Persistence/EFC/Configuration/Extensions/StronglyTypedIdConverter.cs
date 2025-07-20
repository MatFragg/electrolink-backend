using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hampcoders.Electrolink.API.Assets.Infrastructure.Persistence.EFC.Configuration.Extensions;

public class StronglyTypedIdConverter<TId> : ValueConverter<TId, Guid>
    where TId : notnull
{
    private static readonly PropertyInfo _keyProperty =
        typeof(TId).GetProperty("Value")           // preferente
        ?? typeof(TId).GetProperty("Id")              // fallback
        ?? throw new InvalidOperationException(
            $"{typeof(TId).Name} must contain a Guid property named 'Value' or 'Id'.");

    private static readonly ConstructorInfo _ctor =
        typeof(TId).GetConstructor(new[] { typeof(Guid) })
        ?? throw new InvalidOperationException(
            $"{typeof(TId).Name} needs a public ctor(Guid).");

    public StronglyTypedIdConverter()
        : base(
            id   => (Guid)_keyProperty.GetValue(id)!,
            guid => (TId)_ctor.Invoke(new object?[] { guid })!)
    { }
}