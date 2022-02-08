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

            builder.ToTable("resource")
                .HasKey(r => r.ResourceId);

            builder.Property(r => r.ResourceId)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .HasColumnName("name")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);
            
            builder.AddActiveEntityProperties(true);
            builder.AddTrackedEntityProperties(true);
        }
    }
}