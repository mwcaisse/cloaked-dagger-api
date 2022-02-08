using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("user")
                .HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                .HasColumnName("username")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(u => u.EmailVerified)
                .HasColumnName("email_verified")
                .IsRequired();

            builder.Property(u => u.Locked)
                .HasColumnName("locked")
                .IsRequired();

            builder.AddActiveEntityProperties(true);
            builder.AddTrackedEntityProperties(true);

        }
    }
}