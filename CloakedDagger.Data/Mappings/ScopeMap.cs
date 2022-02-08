using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ScopeMap : IEntityTypeConfiguration<ScopeEntity>
    {
        public void Configure(EntityTypeBuilder<ScopeEntity> builder)
        {

            builder.ToTable("scope")
                .HasKey(s => s.ScopeId);

            builder.Property(s => s.ScopeId)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.Name)
                .HasColumnName("name")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(s => s.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            builder.AddActiveEntityProperties(true);
            builder.AddTrackedEntityProperties(true);
        }
    }
}