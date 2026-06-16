using Hampcoders.Electrolink.API.Shared.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hampcoders.Electrolink.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class OrphanedFileDeletionConfiguration : IEntityTypeConfiguration<OrphanedFileDeletion>
{
    public void Configure(EntityTypeBuilder<OrphanedFileDeletion> builder)
    {
        builder.ToTable("pending_cloudinary_deletions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.ProviderId)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Folder)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Reason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.RetryCount)
            .IsRequired();

        builder.Property(x => x.MaxRetries)
            .IsRequired();

        builder.Property(x => x.LastError)
            .HasMaxLength(1000);
    }
}
