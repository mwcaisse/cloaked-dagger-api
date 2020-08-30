using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class ResourceMap : IEntityTypeConfiguration<ResourceEntity>
    {
        public void Configure(EntityTypeBuilder<ResourceEntity> builder)
        {

            builder.ToTable("RESOURCE")
                .HasKey(r => r.ResourceId);

            builder.Property(r => r.ResourceId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .HasColumnName("NAME")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasColumnName("DESCRIPTION")
                .HasMaxLength(1000);
            
            builder.AddActiveEntityProperties();
            builder.AddTrackedEntityProperties();
        }
    }
}