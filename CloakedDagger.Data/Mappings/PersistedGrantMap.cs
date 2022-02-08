using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloakedDagger.Data.Mappings
{
    public class PersistedGrantMap : IEntityTypeConfiguration<PersistedGrantEntity>
    {
        public void Configure(EntityTypeBuilder<PersistedGrantEntity> builder)
        {

            builder.ToTable("persisted_grant")
                .HasKey(pg => pg.Id);

            builder.Property(pg => pg.Id)
                .HasColumnName("id")
                .HasMaxLength(200);

            builder.Property(pg => pg.Type)
                .HasColumnName("type")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pg => pg.SubjectId)
                .HasColumnName("subject_id")
                .HasMaxLength(200);
            
            builder.Property(pg => pg.SessionId)
                .HasColumnName("session_id")
                .HasMaxLength(200);
            
            builder.Property(pg => pg.ClientId)
                .HasColumnName("client_id")
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(pg => pg.Description)
                .HasColumnName("description")
                .HasMaxLength(200);

            builder.Property(pg => pg.CreateDate)
                .HasColumnName("create_date")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(pg => pg.ExpirationDate)
                .HasColumnName("expiration_date");

            builder.Property(pg => pg.ConsumedDate)
                .HasColumnName("consumed_date");

            builder.Property(pg => pg.Data)
                .HasColumnName("data")
                .IsRequired();

        }
    }
}