using Hampcoders.Electrolink.API.IAM.Domain.Model.Aggregates;
using Hampcoders.Electrolink.API.IAM.Domain.Model.ValueObjects;
using Hampcoders.Electrolink.API.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hampcoders.Electrolink.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(u => u.Id);

        builder.Entity<User>()
            .Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();

        builder.Entity<User>()
            .OwnsOne(u => u.Email, e =>
            {
                e.WithOwner()
                    .HasForeignKey("Id");
                e.HasKey("Id");

                e.Property(x => x.Value)
                    .HasColumnName("Email")
                    .IsRequired();

                e.HasIndex(x => x.Value)
                    .IsUnique();
            });
    }
}
