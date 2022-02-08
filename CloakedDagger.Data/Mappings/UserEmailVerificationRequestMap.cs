using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OwlTin.Common.Data;

namespace CloakedDagger.Data.Mappings
{
    public class UserEmailVerificationRequestMap : IEntityTypeConfiguration<UserEmailVerificationRequestEntity>
    {
        public void Configure(EntityTypeBuilder<UserEmailVerificationRequestEntity> builder)
        {
            builder.ToTable("user_email_verification_request")
                .HasKey(vr => vr.Id);

            builder.Property(vr => vr.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(vr => vr.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(vr => vr.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(vr => vr.ValidationKey)
                .HasColumnName("validation_key")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(vr => vr.Successful)
                .HasColumnName("successful")
                .IsRequired();

            builder.HasOne(vr => vr.User)
                .WithMany()
                .HasForeignKey(vr => vr.UserId)
                .IsRequired();
            
            builder.AddActiveEntityProperties(true);
            builder.AddTrackedEntityProperties(true);

        }
    }
}