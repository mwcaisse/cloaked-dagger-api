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
            builder.ToTable("user_role")
                .HasKey(ur => ur.UserRoleId);

            builder.Property(ur => ur.UserRoleId)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(ur => ur.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(ur => ur.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            builder.AddTrackedEntityProperties(true);
        }
    }
}