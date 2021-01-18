using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.ToTable("USER_ROLE")
                .HasKey(ur => ur.UserRoleId);

            builder.Property(ur => ur.UserRoleId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(ur => ur.UserId)
                .HasColumnName("USER_ID")
                .IsRequired();

            builder.Property(ur => ur.RoleId)
                .HasColumnName("ROLE_ID")
                .IsRequired();

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            builder.AddTrackedEntityProperties();
        }
    }
}