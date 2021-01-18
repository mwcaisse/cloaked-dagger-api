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

            builder.ToTable("ROLE")
                .HasKey(r => r.RoleId);

            builder.Property(r => r.RoleId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .HasColumnName("NAME")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasColumnName("DESCRIPTION")
                .HasMaxLength(1000);
            
            builder.AddTrackedEntityProperties();
        }
    }
}