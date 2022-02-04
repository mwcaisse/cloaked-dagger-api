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
            builder.ToTable("USER_EMAIL_VERIFICATION_REQUEST")
                .HasKey(vr => vr.Id);

            builder.Property(vr => vr.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(vr => vr.UserId)
                .HasColumnName("USER_ID")
                .IsRequired();

            builder.Property(vr => vr.Email)
                .HasColumnName("EMAIL")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(vr => vr.ValidationKey)
                .HasColumnName("VALIDATION_KEY")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(vr => vr.Successful)
                .HasColumnName("SUCCESSFUL")
                .IsRequired();

            builder.HasOne(vr => vr.User)
                .WithMany()
                .HasForeignKey(vr => vr.UserId)
                .IsRequired();
            
            builder.AddActiveEntityProperties();
            builder.AddTrackedEntityProperties();

        }
    }
}