using Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>(b =>
        {
            // Mapear el ValueObject `Id` mediante conversión a su valor primitivo.
            // Ajusta `UserId` y `Value` según la implementación real si difieren.
            b.Property(u => u.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserId.From(value))
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedNever();

            // Clave primaria sobre la propiedad mapeada `Id` (ya es un primitivo vía conversión)
            b.HasKey(u => u.Id);

            b.Property(u => u.Email)
                .HasConversion(
                    username => username.Value,
                    value => Email.Create(value)).IsRequired();
            b.Property(u => u.PasswordHash).IsRequired();
        });
    }
}
