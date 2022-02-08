using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {

            builder.ToTable("role")
                .HasKey(r => r.RoleId);

            builder.Property(r => r.RoleId)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .HasColumnName("name")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);
            
            builder.AddTrackedEntityProperties(true);
        }
    }
}