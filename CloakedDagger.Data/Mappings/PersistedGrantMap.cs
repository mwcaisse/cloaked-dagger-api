using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloakedDagger.Data.Mappings
{
    public class PersistedGrantMap : IEntityTypeConfiguration<PersistedGrantEntity>
    {
        public void Configure(EntityTypeBuilder<PersistedGrantEntity> builder)
        {

            builder.ToTable("PERSISTED_GRANT")
                .HasKey(pg => pg.Id);

            builder.Property(pg => pg.Id)
                .HasColumnName("ID")
                .HasMaxLength(200);

            builder.Property(pg => pg.Type)
                .HasColumnName("TYPE")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pg => pg.SubjectId)
                .HasColumnName("SUBJECT_ID")
                .HasMaxLength(200);
            
            builder.Property(pg => pg.SessionId)
                .HasColumnName("SESSION_ID")
                .HasMaxLength(200);
            
            builder.Property(pg => pg.ClientId)
                .HasColumnName("CLIENT_ID")
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(pg => pg.Description)
                .HasColumnName("DESCRIPTION")
                .HasMaxLength(200);

            builder.Property(pg => pg.CreateDate)
                .HasColumnName("CREATE_DATE")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(pg => pg.ExpirationDate)
                .HasColumnName("EXPIRATION_DATE");

            builder.Property(pg => pg.ConsumedDate)
                .HasColumnName("CONSUMED_DATE");

            builder.Property(pg => pg.Data)
                .HasColumnName("DATA")
                .IsRequired();

        }
    }
}