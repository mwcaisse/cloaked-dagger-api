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
            builder.ToTable("USER")
                .HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                .HasColumnName("USERNAME")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasColumnName("PASSWORD")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(u => u.Name)
                .HasColumnName("NAME")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.Email)
                .HasColumnName("EMAIL")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(u => u.EmailVerified)
                .HasColumnName("EMAIL_VERIFIED")
                .IsRequired();

            builder.Property(u => u.Locked)
                .HasColumnName("LOCKED")
                .IsRequired();

            builder.AddActiveEntityProperties();
            builder.AddTrackedEntityProperties();

        }
    }
}